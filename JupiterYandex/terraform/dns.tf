resource "yandex_dns_zone" "dns-zone" {
  folder_id = data.yandex_resourcemanager_folder.folder_net.id
  name             = var.dns-zone
  zone             = "${var.dns-zone}.smartcom.yc."
  public           = false
  private_networks = [data.yandex_vpc_network.network.id]
}

resource "yandex_dns_recordset" "k8s" {
  zone_id = yandex_dns_zone.dns-zone.id
  name    = "k8s"
  type    = "A"
  ttl     = 600
  data    = ["${yandex_kubernetes_cluster.k8s.master[0].internal_v4_address}"]
}

# resource "yandex_dns_recordset" "airflow" {
#   zone_id = yandex_dns_zone.dns-zone.id
#   name    = "airflow"
#   type    = "A"
#   ttl     = 600
#   data    = ["${yandex_kubernetes_cluster.k8s.master[0].internal_v4_address}"]
# }

# resource "yandex_dns_recordset" "superset" {
#   zone_id = yandex_dns_zone.dns-zone.id
#   name    = "superset"
#   type    = "A"
#   ttl     = 600
#   data    = ["${yandex_kubernetes_cluster.k8s.master[0].internal_v4_address}"]
# }