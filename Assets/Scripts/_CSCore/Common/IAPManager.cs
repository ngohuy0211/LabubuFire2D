using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


namespace CompleteProject
{
    //
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        /*Số nhận dạng sản phẩm cho tất cả các sản phẩm có thể được mua: số nhận dạng chung "tiện lợi" để sử dụng với Mua hàng và các mã nhận
         * dạng dành riêng cho cửa hàng của chúng để sử dụng với và bên ngoài Mua hàng Unity. Xác định số nhận dạng dành riêng cho cửa hàng cũng 
         * trên bảng điều khiển dành cho nhà xuất bản của mỗi nền tảng (iTunes Connect, Google Play Developer Console, v.v.)
         */

        /*Số nhận dạng sản phẩm chung cho các sản phẩm tiêu thụ được, không tiêu thụ được và sản phẩm đăng ký. Sử dụng các chốt này trong mã để tham chiếu
         * sản phẩm cần mua. Đồng thời sử dụng các giá trị này khi xác định Số nhận dạng sản phẩm trên cửa hàng. Ngoại trừ, cho mục đích minh họa,
         * kProductIDSubscription - nó có các số nhận dạng tùy chỉnh của Apple và Google. Chúng tôi tuyên bố ánh xạ cửa hàng cụ thể của họ đến Sản phẩm bổ 
         * sung của Unity Purchasing, bên dưới.*/

        public static string kProductIDConsumable = "consumable";
        public static string kProductIDNonConsumable = "nonconsumable";
        public static string kProductIDSubscription = "subscription";

        // Giá trị nhận dạng sản phẩm cụ thể của Apple App Store cho sản phẩm đăng ký.
        private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

        // Sản phẩm đăng ký số nhận dạng sản phẩm cụ thể trên Cửa hàng Google Play.
        private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        void Start()
        {
            // Nếu chúng tôi chưa thiết lập tham chiếu Unity Purchasing
            if (m_StoreController == null)
            {
                // Bắt đầu định cấu hình kết nối của chúng tôi với Mua hàng
                InitializePurchasing();
            }
        }

        public void InitializePurchasing()
        {
            // Nếu chúng tôi đã kết nối với Mua hàng ...
            if (IsInitialized())
            {
                // ... chúng ta đã hoàn tất ở đây.
                return;
            }

            // Tạo một trình tạo, lần đầu tiên đi qua một bộ các cửa hàng do Unity cung cấp.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Thêm sản phẩm để bán / khôi phục bằng số nhận dạng của sản phẩm đó, liên kết số nhận dạng chung với số nhận dạng dành riêng cho cửa hàng của sản phẩm đó.
            builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Tiếp tục thêm sản phẩm không tiêu thụ được.
            builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
            /*Và hoàn tất việc thêm sản phẩm đăng ký. Lưu ý rằng điều này sử dụng ID dành riêng cho cửa hàng, minh họa nếu ID sản phẩm được định cấu hình 
             * khác nhau giữa các cửa hàng của Apple và Google. Cũng lưu ý rằng một người sử dụng tay cầm kProductIDSubscription chung bên trong trò chơi 
             * - ID cửa hàng cụ thể chỉ phải được tham chiếu tại đây. */
            builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
              { kProductNameAppleSubscription, AppleAppStore.Name },
             { kProductNameGooglePlaySubscription, GooglePlay.Name },
            });

            /* Bắt đầu phần còn lại của thiết lập bằng một lệnh gọi không đồng bộ, chuyển cấu hình và phiên bản của lớp này. 
            Mong đợi phản hồi trong OnInitialized hoặc OnInitializeFailed. */
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Chỉ nói rằng chúng tôi được khởi tạo nếu cả hai tham chiếu Mua hàng đều được đặt.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        public void BuyConsumable()
        {
            // Mua sản phẩm tiêu hao bằng cách sử dụng mã nhận dạng chung của nó. Mong đợi phản hồi thông qua ProcessPurchase hoặc OnPurchaseFailed một cách không đồng bộ.
            BuyProductID(kProductIDConsumable);
        }


        public void BuyNonConsumable()
        {
            // Mua sản phẩm không tiêu thụ được bằng cách sử dụng mã nhận dạng chung của nó. Mong đợi phản hồi thông qua ProcessPurchase hoặc OnPurchaseFailed một cách không đồng bộ.
            BuyProductID(kProductIDNonConsumable);
        }


        public void BuySubscription()
        {
            /* Mua sản phẩm đăng ký bằng cách sử dụng số nhận dạng chung của nó.Mong đợi phản hồi thông qua ProcessPurchase hoặc OnPurchaseFailed một cách không đồng bộ.
            Lưu ý cách chúng tôi sử dụng số nhận dạng sản phẩm chung mặc dù ID này được ánh xạ tới các số nhận dạng tùy chỉnh dành riêng cho cửa hàng ở trên. */
            BuyProductID(kProductIDSubscription);
        }


        void BuyProductID(string productId)
        {
            // Nếu Mua hàng đã được khởi tạo ...
            if (IsInitialized())
            {
                // ... tra cứu Tham chiếu sản phẩm với mã định danh sản phẩm chung và bộ sưu tập sản phẩm của Hệ thống mua.
                Product product = m_StoreController.products.WithID(productId);

                // Nếu tra cứu tìm thấy một sản phẩm cho cửa hàng của thiết bị này và sản phẩm đó đã sẵn sàng để bán ...
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... mua sản phẩm. Mong đợi phản hồi thông qua ProcessPurchase hoặc OnPurchaseFailed một cách không đồng bộ.
                    m_StoreController.InitiatePurchase(product);
                }
                // Nếu không thì ...
                else
                {
                    // ... báo cáo tình trạng lỗi tra cứu sản phẩm
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Nếu không thì ...
            else
            {
                // ... báo cáo thực tế là Việc mua chưa khởi chạy thành công. Cân nhắc đợi lâu hơn hoặc thử bắt đầu lại.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }


        // Khôi phục các giao dịch mua trước đó của khách hàng này. Một số nền tảng tự động khôi phục các giao dịch mua, như Google.
        // Apple hiện yêu cầu khôi phục giao dịch mua rõ ràng cho IAP, hiển thị lời nhắc mật khẩu có điều kiện.
        public void RestorePurchases()
        {
            // Nếu Mua chưa được thiết lập ...
            if (!IsInitialized())
            {
                // ... báo cáo tình hình và ngừng khôi phục. Cân nhắc chờ đợi lâu hơn hoặc thử khởi chạy lại.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // Nếu chúng tôi đang chạy trên thiết bị Apple ...
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... bắt đầu khôi phục giao dịch mua
                Debug.Log("RestorePurchases started ...");

                // Tìm nạp hệ thống con dành riêng cho cửa hàng Apple.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Bắt đầu quá trình khôi phục giao dịch mua không đồng bộ. Mong đợi phản hồi xác nhận trong Action <bool> bên dưới và ProcessPurchase nếu
                //có các sản phẩm đã mua trước đó để khôi phục.
                apple.RestoreTransactions((result) =>
                {
                    // Giai đoạn đầu tiên của quá trình khôi phục. Nếu không nhận được thêm phản hồi nào trên ProcessPurchase thì sẽ không có giao dịch mua nào được khôi phục.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Nếu không thì ...
            else
            {
                // Chúng tôi không chạy trên thiết bị Apple. Không cần thực hiện công việc nào để khôi phục các giao dịch mua.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Quá trình mua đã khởi tạo thành công. Thu thập tài liệu tham khảo Mua hàng của chúng tôi.
            Debug.Log("OnInitialized: PASS");

            // Hệ thống mua tổng thể, được cấu hình với các sản phẩm cho ứng dụng này.
            m_StoreController = controller;
            // Lưu trữ hệ thống con cụ thể, để truy cập các tính năng cửa hàng dành riêng cho thiết bị.
            m_StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Thiết lập mua không thành công. Kiểm tra lỗi để biết lý do. Cân nhắc chia sẻ lý do này với người dùng.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            // Thiết lập mua không thành công. Kiểm tra lỗi để biết lý do. Cân nhắc chia sẻ lý do này với người dùng.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
            Debug.Log("OnInitializeFailed InitializationFailureReasonMessage:" + message);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // Người dùng này đã mua một sản phẩm tiêu hao.
            if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // Đã mua thành công vật phẩm tiêu hao, thêm 100 xu vào điểm trong trò chơi của người chơi.
                //  ScoreManager.score += 100;
            }
            // Hoặc ... người dùng này đã mua sản phẩm không tiêu thụ được.
            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // VIỆC LÀM: Vật phẩm không tiêu hao đã được mua thành công, cấp vật phẩm này cho người chơi.
            }
            // Hoặc ... một sản phẩm đăng ký đã được mua bởi người dùng này.
            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // VIỆC CẦN LÀM: Đã mua thành công vật phẩm đăng ký, cấp quyền này cho người chơi.
            }
            // Hoặc ... một sản phẩm không xác định đã được mua bởi người dùng này. Điền vào các sản phẩm bổ sung tại đây ....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            /*Trả lại cờ cho biết liệu sản phẩm này đã được nhận hoàn toàn hay chưa hoặc ứng dụng có cần được nhắc nhở về việc mua hàng này vào lần
             * khởi chạy ứng dụng tiếp theo hay không. Sử dụng PurchaseProcessingResult.Pending khi vẫn còn lưu các sản phẩm đã mua vào đám mây và 
             * khi quá trình lưu đó bị trì hoãn.*/
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            //Một nỗ lực mua sản phẩm đã không thành công. Kiểm tra thất bại Lý do để biết thêm chi tiết. Cân nhắc chia sẻ lý do này với người dùng 
            //để hướng dẫn các hành động khắc phục sự cố của họ.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
}