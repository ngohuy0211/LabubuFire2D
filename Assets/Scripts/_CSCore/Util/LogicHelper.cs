using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicHelper
{
    // hàm này tạo 1 hàm update giả bằng coroutine =)) không chắc là tốt nên thử xem ổn ko
    // time update = time update mỗi lần, timedelay = chờ tối đa bao nhiêu second thì dừng
    public static IEnumerator UpdateByTime(float timeUpdate, float timeDelay, System.Action cb)
    {
        Debug.LogError("sao khong vao day");
        if (timeUpdate > timeDelay)
        {
            Debug.LogError("Có vẻ thời gian nhập vào không hợp lệ");
            yield return null;
        }
        int count = (int)(timeDelay / timeUpdate);
        // truong hop count = 0 thi cho no goi 1 lan
        cb.Invoke();
        Debug.LogError("Có invole khong");
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(timeUpdate);
            cb?.Invoke();
            Debug.LogError("Có invole khong aaaa");
        }
    }

    
}
