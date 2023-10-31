resource "yandex_iam_service_account" "airflow-sa" {
  name        = var.airflow-sa-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "bucket-editor" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "storage.editor"
  members = [
    "serviceAccount:${yandex_iam_service_account.airflow-sa.id}"
  ]
}

resource "yandex_iam_service_account_static_access_key" "airflow-sa-key" {
  service_account_id = yandex_iam_service_account.airflow-sa.id
}

resource "helm_release" "airflow-logs-pvc" {
  namespace        = "airflow"
  create_namespace = true
  name             = "airflow-logs-pvc"
  chart            = "./pvc/helm"
  wait             = true
  depends_on = [
  helm_release.s3-storageClass
  ]
  set {
    name  = "volumeName"
    value = "airflow-logs"
  }
  set {
    name  = "pvcName"
    value = "airflow-logs-pvc"
  }
  set {
    name  = "bucket"
    value = yandex_storage_bucket.bucket.bucket
  }
  set {
    name  = "namespace"
    value = "airflow"
  }
  set {
    name  = "size"
    value = "10Gi"
  }
}

resource "helm_release" "airflow-dags-pvc" {
  namespace        = "airflow"
  create_namespace = true
  name             = "airflow-dags-pvc"
  chart            = "./pvc/helm"
  wait             = true
  depends_on = [
  helm_release.s3-storageClass
  ]
  set {
    name  = "volumeName"
    value = var.airflow-dags-pv
  }
  set {
    name  = "pvcName"
    value = var.airflow-dags-pvc
  }
  set {
    name  = "bucket"
    value = yandex_storage_bucket.bucket.bucket
  }
  set {
    name  = "namespace"
    value = "airflow"
  }
  set {
    name  = "size"
    value = "5Gi"
  }
}

# resource "helm_release" "airflow" {
#   namespace        = "airflow"
#   create_namespace = true
#   name             = "airflow"
#   repository       = "https://airflow.apache.org"
#   chart            = "airflow"
#   wait             = false
#   timeout    = 600
#   depends_on = [
#   helm_release.airflow-logs-pvc,
#   helm_release.airflow-dags-pvc,
#   null_resource.vault-policy
#   ]
#   values = [
#     templatefile("airflow-values.yaml", {
#         subnetId = yandex_vpc_subnet.subnet.id
#         image = var.airflow-image
#         tag = var.airflow-tag
#         pvName = var.airflow-dags-pv
#         pvcName = var.airflow-dags-pvc
#         dataprocUri = "hdfs://rc1b-dataproc-m-i8l6iml0qs1dx7o3.mdb.yandexcloud.net"
#         })
#   ]
# }