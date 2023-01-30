import pendulum

from airflow import DAG
from airflow.operators.trigger_dagrun import TriggerDagRunOperator
from airflow.operators.bash import BashOperator
from airflow.utils.trigger_rule import TriggerRule
from airflow.decorators import dag, task
import uuid
from datetime import datetime

TAGS = ["jupiter", "dev","interface"]

@task(task_id='generate_handler_id')
def generate_handler_id():
    return str(uuid.uuid4())

with DAG(
    dag_id="jupiter_interface_dispatcher",
    start_date=datetime(2023, 1, 30),
    catchup=False,
    schedule_interval='0 8 * * *',
    tags=TAGS,
) as dag:
    handler_id=generate_handler_id()
    
    trigger_jupiter_incoming_file_collect = TriggerDagRunOperator(
        task_id="trigger_jupiter_incoming_file_collect",
        trigger_dag_id="jupiter_incoming_file_collect_azure",
        conf={"parent_handler_id":"{{ti.xcom_pull(task_ids='generate_handler_id')}}"},
        wait_for_completion = True,
    )
    
    handler_id >> trigger_jupiter_incoming_file_collect
    
   
