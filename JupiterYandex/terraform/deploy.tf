resource "yandex_iam_service_account" "deploy-sa" {
 name        = var.deploy-sa-name
 description = "Deployment service account"
 folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "deploy-sa-k8s-api-admin" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "k8s.cluster-api.cluster-admin"
 members   = [
   "serviceAccount:${yandex_iam_service_account.deploy-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "deploy-sa-k8s-viewer" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "k8s.viewer"
 members   = [
   "serviceAccount:${yandex_iam_service_account.deploy-sa.id}"
 ]
}

resource "yandex_resourcemanager_folder_iam_binding" "deploy-sa-pusher" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "container-registry.images.pusher"
 members   = [
   "serviceAccount:${yandex_iam_service_account.deploy-sa.id}"
 ]
}

resource "yandex_iam_service_account_key" "deploy-sa-auth-key" {
  service_account_id = yandex_iam_service_account.deploy-sa.id
  key_algorithm      = "RSA_4096"
}