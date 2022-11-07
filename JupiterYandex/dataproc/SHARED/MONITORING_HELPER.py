# Databricks notebook source
# DBTITLE 0,Monitoring helper fuctions
# MAGIC %md <h3>Monitoring helper functions.</h3></br>
# MAGIC 
# MAGIC Developer: LLC Smart-Com, alexei.loktev@effem.com

# COMMAND ----------

import pandas as pd


def monitoring_df_make():
    """
    Creates new monitoring dataframe.
    :return: pandas.DataFrame
    """
    return pd.DataFrame(
        columns=['PipelineRunId', 'Schema', 'EntityName', 'TargetPath', 'TargetFormat', 'StartDate', 'Duration',
                 'Status', 'ErrorDescription'])


def monitoring_df_append(monitoring_df,
                         pipleline_runid,
                         entity_name,
                         schema,
                         target_path,
                         target_format,
                         start_date,
                         duration,
                         status,
                         error_message=None
                         ):
    """
    Append new row to monitoring dataframe, then return altered dataframe.

    :param monitoring_df: pandas.DataFrame
    :param pipleline_runid:
    :param entity_name:
    :param schema:
    :param target_path:
    :param target_format:
    :param start_date:
    :param duration:
    :param status:
    :param error_message:
    :return: pandas.DataFrame
    """
    row = {
        'PipelineRunId': pipleline_runid
        , 'Schema': schema
        , 'EntityName': entity_name
        , 'TargetPath': target_path
        , 'TargetFormat': target_format
        , 'StartDate': start_date
        , 'Duration': duration
        , 'Status': status
        , 'ErrorDescription': error_message
    }

    return monitoring_df.append(row, ignore_index=True)


def monitoring_df_success(monitoring_df,
                          pipleline_runid,
                          entity_name,
                          schema,
                          target_path,
                          target_format,
                          start_date,
                          duration
                          ):
    """
    Append new row with successfull state to monitoring dataframe, then return altered dataframe.

    :param monitoring_df: pandas.DataFrame
    :param pipleline_runid:
    :param entity_name:
    :param schema:
    :param target_path:
    :param target_format:
    :param start_date:
    :param duration:
    :return: pandas.DataFrame
    """
    return monitoring_df_append(monitoring_df,
                                pipleline_runid,
                                entity_name,
                                schema,
                                target_path,
                                target_format,
                                start_date,
                                duration,
                                'COMPLETE')


def monitoring_df_failure(monitoring_df,
                          pipleline_runid,
                          entity_name,
                          schema,
                          target_path,
                          target_format,
                          start_date,
                          error_message
                          ):
    """
    Append new row with failed state to monitoring dataframe, then return altered dataframe.

    :param monitoring_df: pandas.DataFrame
    :param pipleline_runid:
    :param entity_name:
    :param schema:
    :param target_path:
    :param target_format:
    :param start_date:
    :param error_message:
    :return: pandas.DataFrame
    """
    return monitoring_df_append(monitoring_df,
                                pipleline_runid,
                                entity_name,
                                schema,
                                target_path,
                                target_format,
                                start_date,
                                -1,
                                'FAILURE',
                                error_message)
