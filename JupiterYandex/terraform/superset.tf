resource "helm_release" "superset" {
  namespace        = "superset"
  create_namespace = true
  name             = "superset"
  repository       = "https://apache.github.io/superset"
  version          = "0.8.5" # 0.8.5 last working version with LoadBalancer
  chart            = "superset"
  wait             = true
  timeout    = 600
  depends_on = [
    yandex_kubernetes_node_group.k8s-nodes
  ]
  values = [
    templatefile("superset-values.yaml", {
        subnetId = yandex_vpc_subnet.subnet.id
        })
  ]
}