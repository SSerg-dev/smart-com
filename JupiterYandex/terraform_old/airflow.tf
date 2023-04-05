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

resource "helm_release" "airflow-s3-storageClass" {
  namespace        = "airflow"
  create_namespace = true
  name             = "csi-s3"
  #repository       = "https://github.com/yandex-cloud/k8s-csi-s3"
  #chart            = "csi-s3"
  chart            = "./s3/deploy/helm"
  wait             = true
  depends_on = [
    yandex_kubernetes_node_group.k8s-nodes
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

resource "helm_release" "airflow-pvc" {
  namespace        = "airflow"
  create_namespace = true
  name             = "airflow-pvc"
  chart            = "./pvc/helm"
  wait             = true
  depends_on = [
  helm_release.airflow-s3-storageClass
  ]
  set {
    name  = "name"
    value = "airflow-source-code"
  }
}

resource "helm_release" "airflow" {
  namespace        = "airflow"
  create_namespace = true
  name             = "airflow"
  repository       = "https://airflow.apache.org"
  chart            = "airflow"
  wait             = false
  timeout    = 600
  depends_on = [
  helm_release.airflow-pvc,
  helm_release.vault
  ]
  values = [
    templatefile("airflow-values.yaml", {
        subnetId = yandex_vpc_subnet.subnet.id
        airflowRepoPath = var.airflowRepoPath
        })
  ]
  set {
    name  = "airflow.dbMigrations.runAsJob"
    value = "true"
  }
}