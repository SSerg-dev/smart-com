resource "yandex_organizationmanager_group" "admins-group" {
  name            = "${var.folder}-admins"
  organization_id = var.organization_id
}

resource "yandex_organizationmanager_group_membership" group {
  group_id = yandex_organizationmanager_group.admins-group.id
  members  = var.admins
}

resource "yandex_resourcemanager_cloud_iam_binding" "admins-group-cloud-binding" {
  cloud_id = var.cloud_id
  role = "resource-manager.clouds.member"
  members = [
    "group:${yandex_organizationmanager_group.admins-group.id}",
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "admins-group-folder-binding-1" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role = "k8s.cluster-api.cluster-admin"
  members = [
   "group:${yandex_organizationmanager_group.admins-group.id}",
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "admins-group-folder-binding-2" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role = "viewer"
  members = [
   "group:${yandex_organizationmanager_group.admins-group.id}",
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "admins-group-folder-binding-3" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role = "dataproc.user"
  members = [
   "group:${yandex_organizationmanager_group.admins-group.id}",
  ]
}