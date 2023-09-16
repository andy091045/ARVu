//using System.Collections.Generic;
//using UnityEngine;
//using Firebase;
//using Firebase.Extensions;
//using Firebase.Storage;
//using System;

//public class ListAllFilesInFolder : MonoBehaviour
//{
//    // Firebase Storage 参考
//    private Firebase.Storage.StorageReference folderRef;

//    private void Start()
//    {
//        // 初始化 Firebase
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
//            FirebaseApp app = FirebaseApp.DefaultInstance;
//            FirebaseStorage storage = FirebaseStorage.DefaultInstance;

//            // 设置 Firebase Storage 参考到您的资料夹
//            string folderPath = "your-folder-path"; // 替换为您的资料夹路径
//            folderRef = storage.RootReference.Child(folderPath);

//            // 列举并打印所有檔案的路径
//            ListAllFiles();
//        });
//    }

//    private async void ListAllFiles()
//    {
//        try
//        {
//            // 列举资料夹内的所有檔案和子文件夹
//            Firebase.Storage.StorageReference[] allItems = await folderRef.ListItemsAsync();

//            // 打印檔案的路径
//            foreach (Firebase.Storage.StorageReference item in allItems)
//            {
//                if (item is Firebase.Storage.StorageReference fileRef)
//                {
//                    // 打印檔案的路径
//                    string filePath = fileRef.Path;
//                    Debug.Log("File path: " + filePath);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.LogError("Error listing files: " + ex.Message);
//        }
//    }
//}
