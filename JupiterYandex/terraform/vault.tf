resource "yandex_iam_service_account" "vault-sa" {
  name        = var.vault-sa-name
  folder_id = yandex_resourcemanager_folder.folder.id
}

resource "yandex_kms_symmetric_key" "vault-kms-key" {
  folder_id = yandex_resourcemanager_folder.folder.id
  name              = var.vault-kms-name
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
  namespace        = var.k8s-vault-namespace
  create_namespace = true
  name             = "vault"
  repository       = "oci://cr.yandex/yc-marketplace/yandex-cloud/vault/chart"
  chart            = "vault"
  version          = "0.22.0-1"
  wait             = true
  depends_on = [
  yandex_iam_service_account_key.vault-sa-auth-key,
  yandex_kubernetes_node_group.k8s-nodes
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

resource "null_resource" "vault-init" {
    depends_on = [
    helm_release.vault
    ]
    provisioner "local-exec" {
    command = <<EOT
sleep 120
yc managed-kubernetes cluster get-credentials ${var.k8s-name} --internal --folder-name ${var.folder} --cloud-id ${var.cloud_id} --force 
INIT_VALUES=`kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault operator init`
TOKEN=`echo "$INIT_VALUES" | awk '/Initial Root Token:/ {print $4}'`
kubectl create secret generic vault-init -n ${var.k8s-vault-namespace} --from-literal=init-values="$INIT_VALUES" --from-literal=token="$TOKEN" --from-literal=rkey1="$RKEY1" --from-literal=rkey2="$RKEY2" --from-literal=rkey3="$RKEY3"
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault login -no-print=true $TOKEN
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault secrets enable -version=2 kv
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault auth enable kubernetes
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault write auth/kubernetes/config kubernetes_host="${yandex_kubernetes_cluster.k8s.master.0.internal_v4_endpoint}:443"
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault status
    EOT
  }
}

resource "null_resource" "vault-policy" {
    depends_on = [
    null_resource.vault-init
    ]
    provisioner "local-exec" {
    command = <<EOT
yc managed-kubernetes cluster get-credentials ${var.k8s-name} --internal --folder-name ${var.folder} --cloud-id ${var.cloud_id} --force
TOKEN=`kubectl get secret vault-init -n ${var.k8s-vault-namespace} -o jsonpath='{.data.token}' | base64 --decode`
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault login -no-print=true $TOKEN
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault secrets enable -path=secret kv-v2
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault policy write airflow - <<EOF
path "secret/*"
{
  capabilities = ["read"]
}
EOF
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault write auth/kubernetes/role/airflow bound_service_account_names=airflow-worker bound_service_account_namespaces=airflow policies=airflow ttl=20m
    EOT
  }
}