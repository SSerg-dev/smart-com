resource "yandex_iam_service_account" "dataproc-sa" {
  name        = var.dataproc-sa-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "dataproc-agent-binding" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "mdb.dataproc.agent"
  members = [
    "serviceAccount:${yandex_iam_service_account.dataproc-sa.id}"
  ]
}

resource "yandex_resourcemanager_folder_iam_binding" "bucket-creator-binding" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "editor"
  members = [
    "serviceAccount:${yandex_iam_service_account.dataproc-sa.id}"
  ]
}

resource "yandex_iam_service_account_static_access_key" "dataproc-sa-key" {
  service_account_id = yandex_iam_service_account.dataproc-sa.id
}

resource "yandex_storage_bucket" "bucket" {
  depends_on = [
    yandex_resourcemanager_folder_iam_binding.bucket-creator-binding
  ]
  bucket     = var.bucket-name
  access_key = yandex_iam_service_account_static_access_key.dataproc-sa-key.access_key
  secret_key = yandex_iam_service_account_static_access_key.dataproc-sa-key.secret_key
}

resource "yandex_dataproc_cluster" "dataproc" {
  folder_id           = yandex_resourcemanager_folder.folder.id
  bucket              = var.bucket-name
  name                = var.dataproc.name
  service_account_id  = yandex_iam_service_account.dataproc-sa.id
  zone_id             = var.zone
  deletion_protection = true
  ui_proxy = true
  depends_on = [
    yandex_storage_bucket.bucket
  ]
  cluster_config {
    version_id = "2.0"
    hadoop {
      services   = ["HBASE", "HDFS", "YARN", "SPARK", "TEZ", "MAPREDUCE", "HIVE", "ZEPPELIN", "ZOOKEEPER"]
      properties = {
         "pip:Zeppi_Convert" = "0.1.2"
         "conda:koalas" = "1.8.2"
         "conda:python-hdfs" = "2.7.0"
      }
      ssh_public_keys = [
        file("./id_rsa.pub")
      ]
    }
    subcluster_spec {
      name = "master"
      role = "MASTERNODE"
      resources {
        resource_preset_id = var.dataproc.preset_id_master
        disk_type_id       = "network-ssd"
        disk_size          = var.dataproc.disk_master
      }
      subnet_id   = "${yandex_vpc_subnet.subnet.id}"
      hosts_count = 1
    }
    subcluster_spec {
      name = "data"
      role = "DATANODE"
      resources {
        resource_preset_id = var.dataproc.preset_id_data
        disk_type_id       = "network-ssd"
        disk_size          = var.dataproc.disk_data
      }
      subnet_id   = "${yandex_vpc_subnet.subnet.id}"
      hosts_count = 1
    }
    subcluster_spec {
      name = "compute"
      role = "COMPUTENODE"
      resources {
        resource_preset_id = var.dataproc.preset_id_compute
        disk_type_id       = "network-ssd"
        disk_size          = var.dataproc.disk_compute
      }
      subnet_id   = "${yandex_vpc_subnet.subnet.id}"
      hosts_count = var.dataproc.compute_min
      autoscaling_config {
        max_hosts_count = var.dataproc.compute_max
        measurement_duration = 60
        warmup_duration = 60
        stabilization_duration = 120
        preemptible = true
        decommission_timeout = 60
        cpu_utilization_target = 0
      }
    }
  }
}