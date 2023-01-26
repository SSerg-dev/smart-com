#!/bin/sh

today=`date +'%s'`
hdfs dfs -ls $1 | grep "^d\|^-" | while read line ; do
dir_date=$(echo ${line} | awk '{print $6}')
difference=$(( ( ${today} - $(date -d ${dir_date} +%s) ) / ( 24*60*60 ) ))
filePath=$(echo ${line} | awk '{print $8}')

if [ ${difference} -gt "$2" ]; then
    hdfs dfs -rm -r $filePath
fi
done

