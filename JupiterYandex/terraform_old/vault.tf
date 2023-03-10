resource "yandex_iam_service_account" "vault-sa" {
  name        = var.vault-sa-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_kms_symmetric_key" "vault-kms-key" {
  folder_id = yandex_resourcemanager_folder.folder.id
  name              = var.vault-kms-key-name
  description       = "Vault kms key"
  default_algorithm = "AES_256"
  rotation_period   = "24h"
}

resource "yandex_resourcemanager_folder_iam_binding" "vault" {
  folder_id = yandex_resourcemanager_folder.folder.id
  role    = "kms.keys.encrypterDecrypter"
  members = [
    "serviceAccount:${yandex_iam_service_account.vault-sa.id}"
  ]
}

resource "yandex_iam_service_account_key" "vault-sa-auth-key" {
  service_account_id = yandex_iam_service_account.vault-sa.id
}

resource "helm_release" "vault" {
  namespace        = "vault"
  create_namespace = true
  name             = "vault"
  repository       = "oci://cr.yandex/yc-marketplace/yandex-cloud/vault/chart"
  chart            = "vault"
  version          = "0.19.0-1"
  wait             = true
  depends_on = [
  yandex_iam_service_account_key.vault-sa-auth-key
  ]
  values = [
    yamlencode({
      yandexKmsAuthJson = jsonencode({"id": yandex_iam_service_account_key.vault-sa-auth-key.id,
        "service_account_id": yandex_iam_service_account.vault-sa.id,
        "created_at": yandex_iam_service_account_key.vault-sa-auth-key.created_at,
        "key_algorithm": yandex_iam_service_account_key.vault-sa-auth-key.key_algorithm,
        "public_key": yandex_iam_service_account_key.vault-sa-auth-key.public_key,
        "private_key": yandex_iam_service_account_key.vault-sa-auth-key.private_key
        })
      yandexKmsKeyId = yandex_kms_symmetric_key.vault-kms-key.id
    }),
  ]
}