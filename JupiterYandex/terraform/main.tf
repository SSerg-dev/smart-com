terraform {
  required_providers {
    yandex = {
      source = "yandex-cloud/yandex"
    }
    helm = {
      source = "hashicorp/helm"
    }
  }
  backend "s3" {
    endpoint   = "storage.yandexcloud.net"
    # bucket     = "smartcom-env-tf-bucket"
    # region     = "ru-central1-b"
    # key        = "tfstate/jupiter-app-dev.tfstate"
    # access_key = "YCAJEnCKO2cIWZdGuKcp52l6M"
    # secret_key = "YCMD_CQ5NvuQmY-WDvBlB1G7c2zeJKo8FPGfvoEF"
    skip_region_validation      = true
    skip_credentials_validation = true
  }
  required_version = ">= 0.13"
}

provider "yandex" {
  cloud_id  = var.cloud_id
  service_account_key_file = "authorized_key.json"
}

provider "helm" {
  kubernetes {
    host                   = yandex_kubernetes_cluster.k8s.master[0].internal_v4_endpoint
    cluster_ca_certificate = yandex_kubernetes_cluster.k8s.master[0].cluster_ca_certificate
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["k8s", "create-token"]
      command     = "yc"
    }
  }
}

resource "yandex_resourcemanager_folder" "folder" {
  cloud_id    = var.cloud_id
  name        = var.folder
}

data "yandex_resourcemanager_folder" "folder_net" {
  cloud_id    = var.cloud_id
  name        = var.folder_net
}