resource "yandex_container_registry" "registry" {
  name = var.registry-name
  folder_id = yandex_resourcemanager_folder.folder.id
}