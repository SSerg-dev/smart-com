#ifndef H_BCP_IMPORT
#define H_BCP_IMPORT

#include <sql.h>
#include <sqlext.h>
#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <wchar.h>
#include <ctype.h>
#include "bcp_binding.h"

#define DYN_ARRAY_INITIAL_SIZE 5

#define STR_LEN 128 + 1
#define REM_LEN 254 + 1

SQLCHAR tg_schema[STR_LEN];
SQLCHAR tg_catalog[STR_LEN];
SQLCHAR tg_column_name[STR_LEN];
SQLCHAR tg_table_name[STR_LEN];
SQLCHAR tg_type_name[STR_LEN];
SQLCHAR tg_remarks[REM_LEN];
SQLCHAR tg_column_default[STR_LEN];
SQLCHAR is_nullable[STR_LEN];

SQLINTEGER tg_column_size;
SQLINTEGER tg_buffer_length;
SQLINTEGER tg_char_octet_length;
SQLINTEGER tg_position;

SQLSMALLINT tg_data_type;
SQLSMALLINT tg_decimal_digits;
SQLSMALLINT tg_precision;
SQLSMALLINT tg_nullable;
SQLSMALLINT tg_sql_data_type;
SQLSMALLINT tg_datetime_subtype_code;

SQLHSTMT hstmt = NULL;

SQLLEN cb_catalog;
SQLLEN cb_schema;
SQLLEN cb_table_name;
SQLLEN cb_column_name;
SQLLEN cb_data_type;
SQLLEN cb_type_name;
SQLLEN cb_column_size;
SQLLEN cb_buffer_length;
SQLLEN cbDecimalDigits;
SQLLEN cb_precision;
SQLLEN cb_nullable;
SQLLEN cb_remarks;
SQLLEN cb_column_default;
SQLLEN cb_sql_data_type;
SQLLEN cb_datetime_subtype_code;
SQLLEN cb_char_octet_length;
SQLLEN cb_position;
SQLLEN cb_is_nullable;
/**
 * @brief Binds table columns to special data structures, which are subsequently
 *     be used as a buffer when sending rows to the database.
 *
 * @param hdbc  HDBC handle
 * @param table_name table name
 * @param columns_num number of colums
 * @return ColHolderPtr
 */
ColHolderPtr bind_columns(SQLHDBC hdbc, SQLCHAR *table_name, int *columns_num)
{
    SQLHSTMT hstmt = 0;
    SQLRETURN retcode;

    SQLCHAR outstr[1024];
    SQLSMALLINT outstrlen;

    int holder_index = 0;
    int column_holders_size = DYN_ARRAY_INITIAL_SIZE;
    ColHolderPtr column_holders = NULL;

    char *token;
    char *string;
    char *tofree;
    char parts[2][128];
    char *tab_name = NULL;
    char *schema_name = NULL;

    retcode = SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);

    string = strdup(table_name);

    if (string != NULL)
    {
        tofree = string;
        int j = 0;
        while ((token = strsep(&string, ".")) != NULL)
        {
            strcpy(parts[j], token);
            j++;
        }

        if (j > 1)
        {
            tab_name = parts[1];
            schema_name = parts[0];
        }
        else
        {
            tab_name = parts[0];
        }

        free(tofree);
    }
    retcode = SQLColumns(hstmt, NULL, 0, schema_name, SQL_NTS, tab_name, SQL_NTS, NULL, 0);

    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO)
    {
        SQLBindCol(hstmt, 1, SQL_C_CHAR, tg_catalog, STR_LEN, &cb_catalog);
        SQLBindCol(hstmt, 2, SQL_C_CHAR, tg_schema, STR_LEN, &cb_schema);
        SQLBindCol(hstmt, 3, SQL_C_CHAR, tg_table_name, STR_LEN, &cb_table_name);
        SQLBindCol(hstmt, 4, SQL_C_CHAR, tg_column_name, STR_LEN, &cb_column_name);
        SQLBindCol(hstmt, 5, SQL_C_SSHORT, &tg_data_type, 0, &cb_data_type);
        SQLBindCol(hstmt, 6, SQL_C_CHAR, tg_type_name, STR_LEN, &cb_type_name);
        SQLBindCol(hstmt, 7, SQL_C_SLONG, &tg_column_size, 0, &cb_column_size);
        SQLBindCol(hstmt, 8, SQL_C_SLONG, &tg_buffer_length, 0, &cb_buffer_length);
        SQLBindCol(hstmt, 9, SQL_C_SSHORT, &tg_decimal_digits, 0, &cbDecimalDigits);
        SQLBindCol(hstmt, 10, SQL_C_SSHORT, &tg_precision, 0, &cb_precision);
        SQLBindCol(hstmt, 11, SQL_C_SSHORT, &tg_nullable, 0, &cb_nullable);
        SQLBindCol(hstmt, 12, SQL_C_CHAR, tg_remarks, REM_LEN, &cb_remarks);
        SQLBindCol(hstmt, 13, SQL_C_CHAR, tg_column_default, STR_LEN, &cb_column_default);
        SQLBindCol(hstmt, 14, SQL_C_SSHORT, &tg_sql_data_type, 0, &cb_sql_data_type);
        SQLBindCol(hstmt, 15, SQL_C_SSHORT, &tg_datetime_subtype_code, 0, &cb_datetime_subtype_code);
        SQLBindCol(hstmt, 16, SQL_C_SLONG, &tg_char_octet_length, 0, &cb_char_octet_length);
        SQLBindCol(hstmt, 17, SQL_C_SLONG, &tg_position, 0, &cb_position);
        SQLBindCol(hstmt, 18, SQL_C_CHAR, is_nullable, STR_LEN, &cb_is_nullable);

        column_holders = malloc(column_holders_size * sizeof(ColHolder));

        while (SQL_SUCCESS == retcode)
        {

            if (holder_index == column_holders_size)
            {
                int old_array_size = column_holders_size;
                column_holders_size += DYN_ARRAY_INITIAL_SIZE;

                ColHolderPtr realloced_array = realloc(column_holders, column_holders_size * sizeof(ColHolder));
                if (realloced_array != NULL)
                {
                    column_holders = realloced_array;
                    printf("Array capacity was increased\n");
                }
                else
                {
                    free_column_holders(column_holders, old_array_size);
                    return NULL;
                }
            }

            retcode = SQLFetch(hstmt);

            if (retcode == SQL_ERROR || retcode == SQL_SUCCESS_WITH_INFO)
            {
                printf("fetch error!\n");
            }
            if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO)
            {
                printf("%s %s %d %d %d %d \n", tg_column_name, tg_type_name, tg_data_type, tg_sql_data_type, tg_column_size, tg_decimal_digits);

                switch (tg_sql_data_type)
                {
                case SQL_CHAR:
                {
                    imp_status_code result = bind_column_char(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_WCHAR:
                {
                    imp_status_code result = bind_column_char(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_NUMERIC:
                {
                    imp_status_code result = bind_column_numeric(hdbc, column_holders, holder_index, tg_column_size, tg_decimal_digits, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_DECIMAL:
                {
                    imp_status_code result = bind_column_numeric(hdbc, column_holders, holder_index, tg_column_size, tg_decimal_digits, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_INTEGER:
                {
                    imp_status_code result = bind_column_integer(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_FLOAT:
                {
                    imp_status_code result = bind_column_float(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_REAL:
                {
                    imp_status_code result = bind_column_real(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_DATETIME:
                {
                    imp_status_code result = bind_column_datetime(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_VARCHAR:
                {
                    imp_status_code result = bind_column_varchar(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_WVARCHAR:
                {
                    imp_status_code result = bind_column_wvarchar(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_GUID:
                {
                    imp_status_code result = bind_column_guid(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_SS_TIMESTAMPOFFSET:
                {
                    imp_status_code result = bind_column_timestampoffset(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_SS_TIME2:
                {
                    imp_status_code result = bind_column_time(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_BIT:
                {
                    imp_status_code result = bind_column_bit(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }

                case SQL_SMALLINT:
                {
                    imp_status_code result = bind_column_smallint(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_BIGINT:
                {
                    imp_status_code result = bind_column_bigint(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;
                case SQL_TINYINT:
                {
                    imp_status_code result = bind_column_tinyint(hdbc, column_holders, holder_index, tg_column_size, tg_position, tg_sql_data_type);
                    if (result != IMP_SUCCESS)
                    {
                        free_column_holders(column_holders, column_holders_size);
                        return NULL;
                    }
                }
                break;

                default:
                    break;
                }
                holder_index++;
            }
            else
            {
                break;
            }
        }
    }

    SQLFreeHandle(SQL_HANDLE_STMT, hstmt);

    *columns_num = holder_index;

    return column_holders;
}

void print_byte_array(char *bytes, int length)
{
    if (bytes == NULL)
        return;

    for (int j = 0; j < length; j++)
    {
        debug_printf("%.2x ", bytes[j]);
    }
    debug_printf("\n");
}
#endif
