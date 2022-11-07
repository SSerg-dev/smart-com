

# Before infrastructure deployment

## Required to enable Egress NAT for subnet at portal manually (as this feature in preview mode) before or due deployment
## External public ip should be converted to internal ones only for producton environment

* k8s ->    public_ip = false
* superset ->     # yandex.cloud/load-balancer-type: internal, yandex.cloud/subnet-id: "${subnetId}"
* arflow ->       # yandex.cloud/load-balancer-type: internal, yandex.cloud/subnet-id: "${subnetId}"

## Upload correct ssh key (id_rsa.pub) for Dataproc before deployment
## Login with yc cli and generate iam token for "token" variable before start (yc iam create-token)

# After deployment infrastructure

## Initiate end configure Vault

yc managed-kubernetes cluster get-credentials --id{yandex_kubernetes_cluster.k8s.id} --external --folder-id {yandex_resourcemanager_folder.folder.id} --force

kubectl exec --stdin=true vault-0 -n vault -- vault operator init
## SAVE RECOVERY KEYS
kubectl exec --stdin=true vault-0 -n vault -- vault login {Root token}

kubectl exec --stdin=true vault-0 -n vault -- vault status

### Status should be initiated and unsealed

kubectl get pods --selector='app.kubernetes.io/name=vault'

### READY should be 1/1

kubectl exec --stdin=true vault-0 -n vault -- vault auth enable kubernetes

kubectl exec --stdin=true vault-0 -n vault -- vault write auth/kubernetes/config kubernetes_host="{yandex_kubernetes_cluster.k8s.master.0.internal_v4_endpoint}:443"

kubectl exec --stdin=true vault-0 -n vault -- vault secrets enable -version=2 kv

kubectl exec --stdin=true vault-0 -n vault -- vault policy write airflow - <<EOF
      path "secret/data/variables" {
      capabilities = ["read"]
      }
      path "secret/data/connections" {
      capabilities = ["read"]
      }
      EOF

kubectl exec --stdin=true vault-0 -n vault -- vault write auth/kubernetes/role/airflow bound_service_account_names=airflow-webserver bound_service_account_namespaces=aiflow policies=airflow ttl=20m