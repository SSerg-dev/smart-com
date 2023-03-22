#!/bin/bash
yc managed-kafka cluster start --id $kafka_id --cloud-id $cloud_id --folder-id $folder_id
yc compute instance start --id $instance_id --cloud-id $cloud_id --folder-id $folder_id
yc dataproc cluster start --id $dataproc_id --cloud-id $cloud_id --folder-id $folder_id
yc managed-kubernetes cluster start --id $k8s_id --cloud-id $cloud_id --folder-id $folder_id