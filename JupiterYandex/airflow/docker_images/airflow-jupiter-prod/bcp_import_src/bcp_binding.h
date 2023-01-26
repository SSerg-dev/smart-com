#ifndef H_BCP_BINDING
#define H_BCP_BINDING

#include <sql.h>
#include <sqlext.h>
#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <wchar.h>
#include <ctype.h>
#include <msodbcsql.h>
#include <stdbool.h>
#include <iconv.h>
#include <errno.h>

typedef enum
{
    IMP_SUCCESS = 1,
    IMP_ERROR = 0
} imp_status_code;

typedef struct _ColIntegerBinding
{
    int64_t indicator;
    SQLINTEGER data;
} ColIntegerBinding, *ColIntegerBindingPtr;

typedef struct _ColSmallintBinding
{
    int64_t indicator;
    SQLSMALLINT data;
} ColSmallintBinding, *ColSmallintBindingPtr;

typedef struct _ColBigintBinding
{
    int64_t indicator;
    SQLBIGINT data;
} ColBigintBinding, *ColBigintBindingPtr;

typedef struct _ColTinyintBinding
{
    int64_t indicator;
    SQLCHAR data;
} ColTinyintBinding, *ColTinyintBindingPtr;

typedef struct _ColFloatBinding
{
    int64_t indicator;
    SQLFLOAT data;
} ColFloatBinding, *ColFloatBindingPtr;

typedef struct _ColRealData
{
    int64_t indicator;
    SQLREAL data;
} ColRealBinding, *ColRealBindingPtr;

typedef struct _ColGuidData
{
    int64_t indicator;
    SQLGUID data;
} ColGuidBinding, *ColGuidBindingPtr;

typedef struct _ColDatetime2Data
{
    int64_t indicator;
    TIMESTAMP_STRUCT data;
} ColDatetime2Data, *ColDatetime2DataPtr;

typedef struct _ColDatetimeOffsetBinding
{
    int64_t indicator;
    SQL_SS_TIMESTAMPOFFSET_STRUCT data;
} ColDatetimeOffsetBinding, *ColDatetimeOffsetBindingPtr;

typedef struct _ColBitBinding
{
    int64_t indicator;
    char data;
} ColBitBinding, *ColBitBindingPtr;

typedef struct _ColTimeBinding
{
    int64_t indicator;
    SQL_SS_TIME2_STRUCT data;
} ColTimeBinding, *ColTimeBindingPtr;

typedef struct _ColHolder
{
    void *data;
    int data_size;
    int data_type;
    SQLCHAR precision;
    SQLSCHAR scale;
    short is_null;
    bool dynamic;

} ColHolder, *ColHolderPtr;

typedef struct _ColCharacterBinding
{
    int64_t indicator;
    SQLCHAR data[];
} ColCharacterBinding, *ColCharacterBindingPtr;

typedef struct _ColWCharacterBinding
{
    int64_t indicator;
    WCHAR data[];
} ColWCharacterBinding, *ColWCharacterBindingPtr;

typedef struct _ColNumericBinding
{
    int64_t indicator;
    SQL_NUMERIC_STRUCT data;
} ColNumericBinding, *ColNumericBindingPtr;

char *terminator = "\0";

#define NUMERIC_DECIMAL_DELIMETER '.'
#define UTF8_MAX_BYTES_PER_SYMBOL 4

static const struct tagSS_TIMESTAMPOFFSET_STRUCT empty_timestamoffset;

bool debug_print = false;

#define debug_printf(format, ...)                   \
    do                                              \
    {                                               \
        if (debug_print)                            \
            fprintf(stderr, format, ##__VA_ARGS__); \
    } while (0)

/**
 * @brief Converts string to 128-bit integer
 *
 * @param str input string
 * @param sign number sign, 1 as positive, 0 as negative
 * @return __int128_t number without sign
 */
__int128_t atoint128_t(char *str, char *sign)
{
    __int128_t res = 0;
    size_t i = 0;
    *sign = 1;

    if (str[i] == '-')
    {
        ++i;
        *sign = 0;
    }

    if (str[i] == '+')
    {
        ++i;
    }

    int size = strlen(str);
    for (; i < size; ++i)
    {
        const char c = str[i];
        if (!isdigit(c))
        {
            debug_printf("Non-numeric character: %c\n", c);
            return 1;
        }
        res *= 10;
        res += c - '0';
    }

    return res;
}

char *truncate_string(char *src, int length, bool add_null_character)
{
    int l = add_null_character ? length + 1 : length;
    char *dst = malloc(sizeof(char) * l);
    strncpy(dst, (char *)src, length);

    if (add_null_character)
        dst[length] = '\0';

    return dst;
}

void free_column_holders(ColHolderPtr column_holders, int column_holders_size)
{
    if (column_holders == NULL)
        return;

    for (int holder_index = 0; holder_index < column_holders_size; holder_index++)
    {
        if (column_holders[holder_index].data != NULL)
        {
            free(column_holders[holder_index].data);
        }
    }

    free(column_holders);
}

imp_status_code bind_column_char(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColCharacterBindingPtr bind_ptr = malloc(sizeof(ColCharacterBinding) + sizeof(SQLCHAR) * (column_size + 1));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_size = column_size + 1;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, (UCHAR *)terminator, 1, SQLCHARACTER, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_wchar(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    int data_size = column_size * UTF8_MAX_BYTES_PER_SYMBOL;
    ColCharacterBindingPtr bind_ptr = malloc(sizeof(ColCharacterBinding) + sizeof(SQLCHAR) * data_size);
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_size = data_size;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLCHARACTER, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_numeric(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int decimal_digits, int position, short sql_data_type)
{
    ColNumericBindingPtr bind_ptr = malloc(sizeof(ColNumericBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    column_holders[holder_index].precision = column_size;
    column_holders[holder_index].scale = decimal_digits;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLNUMERICN, position);

    if ((bcp_retcode != SUCCEED))
    {
        debug_printf("Bind column failed! Column=%d\n\n", position);
        return IMP_ERROR;
    }

    return IMP_SUCCESS;
}

imp_status_code bind_column_integer(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColIntegerBindingPtr bind_ptr = malloc(sizeof(ColIntegerBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLINTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_float(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColFloatBindingPtr bind_ptr = malloc(sizeof(ColFloatBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLFLTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_real(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColRealBindingPtr bind_ptr = malloc(sizeof(ColRealBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLFLTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_datetime(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColDatetime2DataPtr bind_ptr = malloc(sizeof(ColDatetime2Data));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLDATETIME2N, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_varchar(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColCharacterBindingPtr bind_ptr = malloc(sizeof(ColCharacterBinding) + sizeof(SQLCHAR) * (column_size + 1));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_size = column_size + 1;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, (UCHAR *)terminator, 1, SQLCHARACTER, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_wvarchar(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{

    RETCODE bcp_retcode;
    column_holders[holder_index].data_type = sql_data_type;

    if (column_size == 0)
    {
        column_holders[holder_index].data_size = column_size;
        column_holders[holder_index].dynamic = true;
        column_holders[holder_index].data = NULL;
        bcp_retcode = bcp_bind(hdbc, NULL, 0, 0, NULL, 0, SQLNCHAR, position);
    }
    else
    {
        int data_size = column_size * UTF8_MAX_BYTES_PER_SYMBOL;
        ColCharacterBindingPtr bind_ptr = malloc(sizeof(ColCharacterBinding) + sizeof(SQLCHAR) * data_size);
        column_holders[holder_index].data = bind_ptr;
        column_holders[holder_index].data_size = data_size;
        column_holders[holder_index].dynamic = false;

        bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLCHARACTER, position);
    }

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_guid(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColGuidBindingPtr bind_ptr = malloc(sizeof(ColGuidBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;

    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLUNIQUEID, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_timestampoffset(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColDatetimeOffsetBindingPtr bind_ptr = malloc(sizeof(ColDatetimeOffsetBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLDATETIMEOFFSETN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_time(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColTimeBindingPtr bind_ptr = malloc(sizeof(ColTimeBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLTIMEN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_bit(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColBitBindingPtr bind_ptr = malloc(sizeof(ColBitBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLBITN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_smallint(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColSmallintBindingPtr bind_ptr = malloc(sizeof(ColSmallintBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLINTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_bigint(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColBigintBindingPtr bind_ptr = malloc(sizeof(ColBigintBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLINTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code bind_column_tinyint(SQLHDBC hdbc, ColHolderPtr column_holders, int holder_index, int column_size, int position, short sql_data_type)
{
    ColTinyintBindingPtr bind_ptr = malloc(sizeof(ColTinyintBinding));
    column_holders[holder_index].data = bind_ptr;
    column_holders[holder_index].data_type = sql_data_type;
    RETCODE bcp_retcode = bcp_bind(hdbc, (BYTE *)bind_ptr, 8, SQL_VARLEN_DATA, NULL, 0, SQLINTN, position);

    if ((bcp_retcode != SUCCEED))

        if ((bcp_retcode != SUCCEED))
        {
            debug_printf("Bind column failed! Column=%d\n\n", position);
            return IMP_ERROR;
        }

    return IMP_SUCCESS;
}

imp_status_code write_column_char(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColCharacterBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        memset(bind_ptr->data, 0, columnData[current_col].data_size);
        strncpy(bind_ptr->data, s, holder_index);
        bind_ptr->indicator = (int)strlen(bind_ptr->data);
        debug_printf("%" PRId64 " %s  %s\n", bind_ptr->indicator, bind_ptr->data, (char *)s);
    }
}

imp_status_code write_column_wchar(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColCharacterBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        memset(bind_ptr->data, 0, columnData[current_col].data_size);
        strncpy(bind_ptr->data, s, holder_index);
        bind_ptr->indicator = (int)strlen(bind_ptr->data);
        debug_printf("%" PRId64 " %s  %s\n", bind_ptr->indicator, bind_ptr->data, (char *)s);
    }
}

imp_status_code write_column_numeric(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColNumericBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        bind_ptr->data.precision = columnData[current_col].precision;
        bind_ptr->data.scale = columnData[current_col].scale;
        bind_ptr->data.sign = 0;

        char *temp_str = malloc(sizeof(char) * holder_index);
        strncpy(temp_str, s, holder_index);

        char *dot = strchr(temp_str, NUMERIC_DECIMAL_DELIMETER);
        if (dot != NULL)
        {
            int index = (int)(dot - temp_str);
            memmove(&temp_str[index], &temp_str[index + 1], strlen(temp_str) - index);
        }
        debug_printf("%s\n", temp_str);

        __int128_t value = atoint128_t(temp_str, &bind_ptr->data.sign);

        bind_ptr->data.val[15] = (value >> 120) & 0xFF;
        bind_ptr->data.val[14] = (value >> 112) & 0xFF;
        bind_ptr->data.val[13] = (value >> 104) & 0xFF;
        bind_ptr->data.val[12] = (value >> 96) & 0xFF;

        bind_ptr->data.val[11] = (value >> 88) & 0xFF;
        bind_ptr->data.val[10] = (value >> 80) & 0xFF;
        bind_ptr->data.val[9] = (value >> 72) & 0xFF;
        bind_ptr->data.val[8] = (value >> 64) & 0xFF;

        bind_ptr->data.val[7] = (value >> 56) & 0xFF;
        bind_ptr->data.val[6] = (value >> 48) & 0xFF;
        bind_ptr->data.val[5] = (value >> 40) & 0xFF;
        bind_ptr->data.val[4] = (value >> 32) & 0xFF;

        bind_ptr->data.val[3] = (value >> 24) & 0xFF;
        bind_ptr->data.val[2] = (value >> 16) & 0xFF;
        bind_ptr->data.val[1] = (value >> 8) & 0xFF;
        bind_ptr->data.val[0] = value & 0xFF;

        free(temp_str);
    }
}

imp_status_code write_column_integer(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColIntegerBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = malloc(sizeof(char) * holder_index + 1);
        strncpy(temp_str, (char *)s, holder_index);
        temp_str[holder_index] = '\0';
        bind_ptr->data = strtoimax(temp_str, NULL, 10);
        free(temp_str);

        if (errno == ERANGE)
        {
            debug_printf("%s\n", strerror(errno));
        }
    }
}

imp_status_code write_column_smallint(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColSmallintBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = malloc(sizeof(char) * holder_index + 1);
        strncpy(temp_str, (char *)s, holder_index);
        temp_str[holder_index] = '\0';
        bind_ptr->data = strtoimax(temp_str, NULL, 10);
        free(temp_str);

        if (errno == ERANGE)
        {
            debug_printf("%s\n", strerror(errno));
        }
    }
}

imp_status_code write_column_bigint(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColBigintBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = malloc(sizeof(char) * holder_index + 1);
        strncpy(temp_str, (char *)s, holder_index);
        temp_str[holder_index] = '\0';
        bind_ptr->data = strtoll(temp_str, NULL, 10);
        free(temp_str);

        if (errno == ERANGE)
        {
            debug_printf("%s\n", strerror(errno));
        }
    }
}

imp_status_code write_column_tinyint(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColTinyintBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = malloc(sizeof(char) * holder_index + 1);
        strncpy(temp_str, (char *)s, holder_index);
        temp_str[holder_index] = '\0';
        bind_ptr->data = strtoimax(temp_str, NULL, 10);
        free(temp_str);

        if (errno == ERANGE)
        {
            debug_printf("%s\n", strerror(errno));
        }
    }
}

imp_status_code write_column_float(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColFloatBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = truncate_string(s, holder_index, true);
        bind_ptr->data = strtod(temp_str, NULL);
        free(temp_str);
        debug_printf("%f \n", bind_ptr->data);
    }
}

imp_status_code write_column_real(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColRealBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char *temp_str = truncate_string(s, holder_index, true);
        bind_ptr->data = strtof(temp_str, NULL);
        free(temp_str);
        debug_printf("%f \n", bind_ptr->data);
    }
}

imp_status_code write_column_datetime(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColDatetime2DataPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        char *temp_str = truncate_string((char *)s, holder_index, true);
        bind_ptr->indicator = sizeof(bind_ptr->data);
        debug_printf("length=%d source=%s copy=%s\n", holder_index, (char *)s, temp_str);

        sscanf(temp_str, "%hu-%hu-%hu %hu:%hu:%hu.%d;",
               &bind_ptr->data.year,
               &bind_ptr->data.month,
               &bind_ptr->data.day,
               &bind_ptr->data.hour,
               &bind_ptr->data.minute,
               &bind_ptr->data.second,
               &bind_ptr->data.fraction);
        bind_ptr->data.fraction *= 100;
        free(temp_str);
    }
}

imp_status_code write_column_varchar(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColCharacterBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        memset(bind_ptr->data, 0, columnData[current_col].data_size);
        strncpy(bind_ptr->data, s, holder_index);
        bind_ptr->indicator = (int)strlen(bind_ptr->data);
        debug_printf("%" PRId64 " %s  %s\n", bind_ptr->indicator, bind_ptr->data, (char *)s);
    }
}

imp_status_code write_column_wvarchar(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColCharacterBindingPtr bind_ptr = NULL;

    if (columnData[current_col].dynamic)
    {
        if (columnData[current_col].data != NULL)
        {
            free(columnData[current_col].data);
        }

        int data_size = holder_index * UTF8_MAX_BYTES_PER_SYMBOL;
        bind_ptr = malloc(sizeof(ColCharacterBinding) + sizeof(SQLCHAR) * data_size);
        if (s == NULL)
        {
            bind_ptr->indicator = SQL_NULL_DATA;
            debug_printf("Set null data\n");
        }
        else
        {
            memset(bind_ptr->data, 0, data_size);
            strncpy(bind_ptr->data, s, holder_index);
            bind_ptr->indicator = (int)strlen(bind_ptr->data);
            debug_printf("%" PRId64 "  {%s}  {%s}\n", holder_index, bind_ptr->data, (char *)s);
        }

        columnData[current_col].data = bind_ptr;
        return IMP_SUCCESS;
    }

    bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        memset(bind_ptr->data, 0, columnData[current_col].data_size);
        strncpy(bind_ptr->data, s, holder_index);
        bind_ptr->indicator = (int)strlen(bind_ptr->data);
        debug_printf("%" PRId64 "  %s  %s\n", bind_ptr->indicator, bind_ptr->data, (char *)s);
    }
}

imp_status_code write_column_guid(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColGuidBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        char *temp_str = truncate_string(s, holder_index, true);
        debug_printf("indicator=%" PRId64 "  source=%s  copy=%s\n", bind_ptr->indicator, (char *)s, temp_str);
        bind_ptr->indicator = sizeof(bind_ptr->data);

        uuid_t id;
        uuid_parse(temp_str, id);
        bind_ptr->data.Data1 = id[3] + (id[2] << 8) + (id[1] << 16) + (id[0] << 24);
        bind_ptr->data.Data2 = id[5] + (id[4] << 8);
        bind_ptr->data.Data3 = id[7] + (id[6] << 8);
        bind_ptr->data.Data4[0] = id[8];
        bind_ptr->data.Data4[1] = id[9];
        bind_ptr->data.Data4[2] = id[10];
        bind_ptr->data.Data4[3] = id[11];
        bind_ptr->data.Data4[4] = id[12];
        bind_ptr->data.Data4[5] = id[13];
        bind_ptr->data.Data4[6] = id[14];
        bind_ptr->data.Data4[7] = id[15];

        free(temp_str);
    }
}

imp_status_code write_column_timestampoffset(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColDatetimeOffsetBindingPtr bind_ptr = columnData[current_col].data;

    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        bind_ptr->data = empty_timestamoffset;

        char *temp_str = truncate_string(s, holder_index, true);
        sscanf(temp_str, "%hu-%hu-%hu %hu:%hu:%hu.%d %3hu:%2hu;",
               &bind_ptr->data.year,
               &bind_ptr->data.month,
               &bind_ptr->data.day,
               &bind_ptr->data.hour,
               &bind_ptr->data.minute,
               &bind_ptr->data.second,
               &bind_ptr->data.fraction,
               &bind_ptr->data.timezone_hour,
               &bind_ptr->data.timezone_minute

        );

        bind_ptr->data.fraction *= 100;
        free(temp_str);
    }
}

imp_status_code write_column_time(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColTimeBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        sscanf(s, "%hu:%hu:%hu.%d",
               &bind_ptr->data.hour,
               &bind_ptr->data.minute,
               &bind_ptr->data.second,
               &bind_ptr->data.fraction);

        bind_ptr->data.fraction *= 100;
    }
}

imp_status_code write_column_bit(ColHolderPtr columnData, char *s, int holder_index, int current_col)
{
    ColBitBindingPtr bind_ptr = columnData[current_col].data;
    if (s == NULL)
    {
        bind_ptr->indicator = SQL_NULL_DATA;
    }
    else
    {
        bind_ptr->indicator = sizeof(bind_ptr->data);
        char temp_str[2];
        strncpy(temp_str, s, 1);
        temp_str[1] = '\n';
        bind_ptr->data = strtoimax(temp_str, NULL, 10);
        if (errno == ERANGE)
        {
            debug_printf("%s\n", strerror(errno));
        }
    }
}

/**
 *
 **/

void write_dynamic_column_wvarchar(SQLHDBC hdbc, ColHolderPtr columnData, int current_col, iconv_t conv_utf8_to_utf16le, char *conv_buffer, size_t conv_buffer_size, int chunk_max_len)
{
    ColCharacterBindingPtr bind_ptr = columnData[current_col].data;
    if (bind_ptr->indicator == SQL_NULL_DATA)
    {
        if (bcp_moretext(hdbc, SQL_NULL_DATA, NULL) == FAIL)
        {
            debug_printf("Moretext failed\n");
            return;
        }
    }
    else
    {

        int converted_size = strlen(bind_ptr->data);
        debug_printf("Original data(UTF-8). Size=%d Data={%s}\n", converted_size, bind_ptr->data);

        size_t conv_result, input_len, output_len;
        int conv_error_code;

        char *input_str = bind_ptr->data;
        char *output_str = conv_buffer;
        input_len = strlen(input_str);
        output_len = conv_buffer_size;
        memset(conv_buffer, 0, conv_buffer_size);
        debug_printf("Conv buffer size: %ld\n", output_len);
        errno = 0;
        conv_result = iconv(conv_utf8_to_utf16le, &input_str, &input_len, &output_str, &output_len);
        conv_error_code = errno;
        debug_printf("Converted: %u,error=%d\n", (unsigned)conv_result, conv_error_code);

        size_t total_len = conv_buffer_size - output_len;
        debug_printf("Converted data(UTF-16LE). Size=%ld\n", total_len);

        int remainder = total_len % chunk_max_len;
        int chunks = total_len / chunk_max_len;
        debug_printf("Chunks=%d\n", chunks);
        chunks = remainder != 0 ? chunks + 1 : chunks;

        for (int ch = 0, off = 0; ch < chunks; ch++, off += chunk_max_len)
        {
            int chunk_len = ch == chunks - 1 && remainder != 0 ? remainder : chunk_max_len;
            char *start_byte = &conv_buffer[off];
            debug_printf("Offset=%d chunk length= %d\n", off, chunk_len);

            // print_byte_array(start_byte, chunk_len);
            if (bcp_moretext(hdbc, chunk_len, start_byte) == FAIL)
            {
                debug_printf("Moretext failed\n");
                return;
            }
        }
        if (bcp_moretext(hdbc, 0, NULL) == FAIL)
        {
            printf("Moretext ending failed\n");
            return;
        }
    }
}

#endif