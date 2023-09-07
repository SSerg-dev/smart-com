resource "yandex_iam_service_account" "datasphere-sa" {
  name        = var.datasphere-sa-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "datasphere-sa-dp-agent" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "dataproc.agent"
  members = [
    "serviceAccount:${yandex_iam_service_account.datasphere-sa.id}"
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "datasphere-sa-dp-admin" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "dataproc.admin"
  members = [
    "serviceAccount:${yandex_iam_service_account.datasphere-sa.id}"
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "datasphere-sa-iam-user" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "iam.serviceAccounts.user"
  members = [
    "serviceAccount:${yandex_iam_service_account.datasphere-sa.id}"
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "datasphere-sa-vpc-user" {
  folder_id = data.yandex_resourcemanager_folder.folder_net.id
  role    = "vpc.user"
  members = [
    "serviceAccount:${yandex_iam_service_account.datasphere-sa.id}"
  ]
}