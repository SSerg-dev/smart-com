locals {
  registry-url = "cr.yandex/${yandex_container_registry.registry.id}"
  k8s-ip = "${yandex_kubernetes_cluster.k8s.master[0].internal_v4_endpoint}"
  deploy-sa-key-json = jsonencode({"id": yandex_iam_service_account_key.deploy-sa-auth-key.id,
        "service_account_id": yandex_iam_service_account.deploy-sa.id,
        "created_at": yandex_iam_service_account_key.deploy-sa-auth-key.created_at,
        "key_algorithm": yandex_iam_service_account_key.deploy-sa-auth-key.key_algorithm,
        "public_key": yandex_iam_service_account_key.deploy-sa-auth-key.public_key,
        "private_key": yandex_iam_service_account_key.deploy-sa-auth-key.private_key})
}

resource "null_resource" "vault-secrets" {
    depends_on = [
    null_resource.vault-policy,
    yandex_kubernetes_cluster.k8s,
    yandex_container_registry.registry
    ]
    provisioner "local-exec" {
    command = <<EOT
yc managed-kubernetes cluster get-credentials ${var.k8s-name} --internal --folder-name ${var.folder} --cloud-id ${var.cloud_id} --force
TOKEN=`kubectl get secret vault-init -n ${var.k8s-vault-namespace} -o jsonpath='{.data.token}' | base64 --decode`
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault login -no-print=true $TOKEN
kubectl exec --stdin=true vault-0 -n ${var.k8s-vault-namespace} -- vault kv put secret/ConnectionStrings K8s="${local.k8s-ip}" Registry="${local.registry-url}"
    EOT
  }
}

resource "null_resource" "deploy-sa-secret" {
    depends_on = [
      yandex_iam_service_account_key.deploy-sa-auth-key,
      null_resource.vault-policy
    ]
    provisioner "local-exec" {
    command = <<EOT
yc managed-kubernetes cluster get-credentials ${var.k8s-name} --internal --folder-name ${var.folder} --cloud-id ${var.cloud_id} --force
kubectl create secret generic deploy-sa -n ${var.k8s-vault-namespace} --from-literal=json='${local.deploy-sa-key-json}'
    EOT
  }
}

output "k8s-ip" {
  value = local.k8s-ip
}

output "registry-url" {
  value = local.registry-url
}