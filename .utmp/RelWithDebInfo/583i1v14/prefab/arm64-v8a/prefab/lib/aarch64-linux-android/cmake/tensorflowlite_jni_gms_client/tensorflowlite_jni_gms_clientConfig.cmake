if(NOT TARGET tensorflowlite_jni_gms_client::tensorflowlite_jni_gms_client)
add_library(tensorflowlite_jni_gms_client::tensorflowlite_jni_gms_client SHARED IMPORTED)
set_target_properties(tensorflowlite_jni_gms_client::tensorflowlite_jni_gms_client PROPERTIES
    IMPORTED_LOCATION "C:/Users/Windows/.gradle/caches/8.11/transforms/e381ec72e652911b887f1e583a9be91e/transformed/jetified-play-services-tflite-java-16.4.0/prefab/modules/tensorflowlite_jni_gms_client/libs/android.arm64-v8a/libtensorflowlite_jni_gms_client.so"
    INTERFACE_INCLUDE_DIRECTORIES "C:/Users/Windows/.gradle/caches/8.11/transforms/e381ec72e652911b887f1e583a9be91e/transformed/jetified-play-services-tflite-java-16.4.0/prefab/modules/tensorflowlite_jni_gms_client/include"
    INTERFACE_LINK_LIBRARIES ""
)
endif()

