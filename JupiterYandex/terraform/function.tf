resource "yandex_iam_service_account" "function-sa" {
 name        = var.function-sa-name
 description = "Service account for start-stop functions"
 folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_resourcemanager_folder_iam_binding" "function-editor" {
 folder_id = yandex_resourcemanager_folder.folder.id
 role      = "editor"
 members   = [
   "serviceAccount:${yandex_iam_service_account.function-sa.id}"
 ]
}

resource "yandex_function" "function-stop" {
    name               = var.function-stop-name
    folder_id = yandex_resourcemanager_folder.folder.id
    description        = "Stop resources"
    runtime            = "bash"
    user_hash          = "v2"
    entrypoint         = "stop.sh"
    memory             = "128"
    execution_timeout  = "300"
    service_account_id = yandex_iam_service_account.function-sa.id
    environment = {
        folder_id = yandex_resourcemanager_folder.folder.id
        cloud_id = var.cloud_id
        k8s_id = yandex_kubernetes_cluster.k8s.id
        dataproc_id = yandex_dataproc_cluster.dataproc.id
        kafka_id = yandex_mdb_kafka_cluster.kafka.id
    }
    content {
        zip_filename = "function.zip"
    }
}

resource "yandex_function_trigger" "function-stop-trigger" {
  name        = var.function-stop-name
  folder_id = yandex_resourcemanager_folder.folder.id
  description = "Stop resources daily"
  timer {
    cron_expression = "0 17 ? * * *"
  }
  function {
    id = yandex_function.function-stop.id
    service_account_id = yandex_iam_service_account.function-sa.id
  }
}