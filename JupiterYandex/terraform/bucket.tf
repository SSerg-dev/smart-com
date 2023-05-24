resource "yandex_storage_bucket" "bucket" {
  depends_on = [
    yandex_resourcemanager_folder_iam_binding.dataproc-sa-editor
  ]
  bucket     = var.bucket-name
  max_size   = var.bucket-size
  access_key = yandex_iam_service_account_static_access_key.dataproc-sa-key.access_key
  secret_key = yandex_iam_service_account_static_access_key.dataproc-sa-key.secret_key
}

resource "helm_release" "s3-storageClass" {
  namespace        = "airflow"
  create_namespace = true
  name             = "csi-s3"
  chart            = "csi-s3"
  repository       = "oci://cr.yandex/yc-marketplace/yandex-cloud/csi-s3"
  version          = "0.31.8"
  wait             = true
  depends_on = [
    yandex_kubernetes_node_group.k8s-nodes,
    yandex_storage_bucket.bucket,
    yandex_iam_service_account_static_access_key.airflow-sa-key
  ]
  set {
    name = "secret.accessKey"
    value = yandex_iam_service_account_static_access_key.airflow-sa-key.access_key
  }  
  set {  
    name = "secret.secretKey"
    value = yandex_iam_service_account_static_access_key.airflow-sa-key.secret_key
  }
  set {  
    name = "storageClass.singleBucket"
    value = var.bucket-name
  }
}