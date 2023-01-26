#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <sql.h>
#include <sqlext.h>
#include <stddef.h>
#include <wchar.h>
#include <inttypes.h>
#include <msodbcsql.h>
#include <csv.h>
#include <uuid.h>
#include <unistd.h>
#include <stdbool.h>
#include "bcp_import.h"
#include <iconv.h>

#define DEFAULT_CONV_BUFFER_SIZE 1048576
SQLHENV henv = SQL_NULL_HENV;
HDBC hdbc = SQL_NULL_HDBC;
int column_num;
RETCODE send_ret;
struct csv_parser p;
int current_col = 0;
int current_row = 0;
int batch_size = -1;
size_t conv_buffer_size = DEFAULT_CONV_BUFFER_SIZE; // Buffer for utf8->utf16le conversion

/**
 * @brief Counter of failed row writes
 *
 */
int row_write_errors = 0;
bool skip_header = false;
bool allow_send_row_failures = false;
ColHolderPtr column_holders = NULL;
unsigned char delimeter = CSV_COMMA;

iconv_t conv_utf8_to_utf16le;
#define CHUNK_MAX_LENGTH 2000
char *conv_buffer = NULL;

void Cleanup()
{
   csv_free(&p);
   if (hdbc != SQL_NULL_HDBC)
   {
      SQLDisconnect(hdbc);
      SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
   }

   if (henv != SQL_NULL_HENV)
      SQLFreeHandle(SQL_HANDLE_ENV, henv);

   free_column_holders(column_holders, column_num);

   iconv_close(conv_utf8_to_utf16le);

   if (conv_buffer != NULL)
      free(conv_buffer);
}

void parser_column_callback(void *s, size_t i, void *p)
{
   if (skip_header && current_row == 0)
      return;

   switch (column_holders[current_col].data_type)
   {
   case SQL_CHAR:
   {
      write_column_char(column_holders, s, i, current_col);
   }
   break;
   case SQL_WCHAR:
   {
      write_column_char(column_holders, s, i, current_col);
   }
   break;
   case SQL_NUMERIC:
   {
      write_column_numeric(column_holders, s, i, current_col);
   }
   break;
   case SQL_DECIMAL:
   {
      write_column_numeric(column_holders, s, i, current_col);
   }
   break;
   case SQL_INTEGER:
   {
      write_column_integer(column_holders, s, i, current_col);
   }
   break;
   case SQL_SMALLINT:
   {
      write_column_smallint(column_holders, s, i, current_col);
   }
   break;
   case SQL_BIGINT:
   {
      write_column_bigint(column_holders, s, i, current_col);
   }
   break;
   case SQL_TINYINT:
   {
      write_column_tinyint(column_holders, s, i, current_col);
   }
   break;
   case SQL_FLOAT:
   {
      write_column_float(column_holders, s, i, current_col);
   }
   break;
   case SQL_REAL:
   {
      write_column_real(column_holders, s, i, current_col);
   }
   break;
   case SQL_DATETIME:
   {
      write_column_datetime(column_holders, s, i, current_col);
   }
   break;
   case SQL_VARCHAR:
   {
      write_column_varchar(column_holders, s, i, current_col);
   }
   break;
   case SQL_WVARCHAR:
   {
      write_column_wvarchar(column_holders, s, i, current_col);
   }
   break;
   case SQL_GUID:
   {
      write_column_guid(column_holders, s, i, current_col);
   }
   break;
   case SQL_SS_TIMESTAMPOFFSET:
   {
      write_column_timestampoffset(column_holders, s, i, current_col);
   }
   break;
   case SQL_SS_TIME2:
   {
      write_column_time(column_holders, s, i, current_col);
   }
   break;
   case SQL_BIT:
   {
      write_column_bit(column_holders, s, i, current_col);
   }
   break;

   default:
      break;
   }

   current_col++;
}

void parser_row_callback(int c, void *p)
{
   if (skip_header && current_row == 0)
   {
      printf("Skiping row: %d\n", current_row);
      current_row++;
      return;
   }

   current_col = 0;
   if ((send_ret = bcp_sendrow(hdbc)) != SUCCEED)
   {
      row_write_errors++;
      printf("Send row failed. Row=%d\n\n", current_row);
   }

   // Dynamic data handling
   for (int c = 0; c < column_num; c++)
   {
      int col_index = c;
      if (column_holders[c].dynamic)
      {
         debug_printf("Dynamic column=%d\n", col_index + 1);
         switch (column_holders[col_index].data_type)
         {
         case SQL_WVARCHAR:
         {
            write_dynamic_column_wvarchar(hdbc, column_holders, col_index, conv_utf8_to_utf16le, conv_buffer, conv_buffer_size, CHUNK_MAX_LENGTH);
         }
         break;

         default:
            break;
         }
      }
   }

   if (current_row % 99999 == 0)
   {
      printf("100000 rows was successfully sent\n");
   }

   current_row++;
}

int main(int argc, char *argv[])
{
   RETCODE retcode;
   SQLCHAR outstr[1024];
   SQLSMALLINT outstrlen;
   DBINT rows_done = 0;
   char connection_string_par[200];
   char table_name_par[128];
   char delimeter_par[10];

   int opt;
   enum
   {
      CHARACTER_MODE,
      WORD_MODE,
      LINE_MODE
   } mode = CHARACTER_MODE;

   while ((opt = getopt(argc, argv, "b:C:sad")) != -1)
   {
      switch (opt)
      {
      case 'b':
         if (optarg != NULL)
         {
            batch_size = strtoimax(optarg, NULL, 10);
            if (batch_size == 0)
               batch_size = -1;
         }
         break;
      case 'C':
         if (optarg != NULL)
         {
            conv_buffer_size = strtoimax(optarg, NULL, 10);
            if (conv_buffer_size == 0)
               conv_buffer_size = DEFAULT_CONV_BUFFER_SIZE;
         }
         break;
      case 's':
         skip_header = true;
         break;
      case 'a':
         allow_send_row_failures = true;
         break;
      case 'd':
         debug_print = true;
         break;
      default:
         fprintf(stderr, "Usage: %s [-b[batch size] -C[conv buffer size] -s -a] \"ODBC connection string\" \"Table name\" [Separator] \n", argv[0]);
         return EXIT_FAILURE;
      }
   }

   if (optind + 1 >= argc)
   {
      fprintf(stderr, "Usage: %s [-b[batch size] -C[conv buffer size] -s -a] \"ODBC connection string\" \"Table name\" [Separator] \n", argv[0]);
      fprintf(stderr, "Example: %s %s\n", argv[0], "\"DRIVER=ODBC Driver 18 for SQL Server;SERVER=192.168.0.1;DATABASE=dbname;UID=user;PWD=password;Encrypt=no;\" \"TAB1\" \t ");
      return EXIT_FAILURE;
   }
   else
   {
      strcpy(connection_string_par, argv[optind]);
      strcpy(table_name_par, argv[optind + 1]);
      if (argv[optind + 2] != NULL)
      {
         strcpy(delimeter_par, argv[optind + 2]);
         delimeter = strtoimax(delimeter_par, NULL, 10);
      }
   }

   conv_utf8_to_utf16le = iconv_open("utf-16le", "utf-8");
   if (conv_utf8_to_utf16le == (iconv_t)(-1))
   {
      printf("Iconv open error\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   printf("Conv buffer size: %ld\n\n", conv_buffer_size);
   conv_buffer = malloc(conv_buffer_size);
   if (conv_buffer == NULL)
   {
      printf("Can't allocate conversion buffer\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = SQLAllocHandle(SQL_HANDLE_ENV, NULL, &henv);
   if ((retcode != SQL_SUCCESS_WITH_INFO) && (retcode != SQL_SUCCESS))
   {
      printf("SQLAllocHandle failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)SQL_OV_ODBC3, SQL_IS_INTEGER);
   if ((retcode != SQL_SUCCESS_WITH_INFO) && (retcode != SQL_SUCCESS))
   {
      printf("SQLSetEnvAttr failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);
   if ((retcode != SQL_SUCCESS_WITH_INFO) && (retcode != SQL_SUCCESS))
   {
      printf("SQLAllocHandle failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = SQLSetConnectAttr(hdbc, SQL_COPT_SS_BCP, (void *)SQL_BCP_ON, SQL_IS_INTEGER);
   if ((retcode != SQL_SUCCESS_WITH_INFO) && (retcode != SQL_SUCCESS))
   {
      printf("SQLSetConnectAttr failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = SQLDriverConnect(hdbc, NULL, connection_string_par, SQL_NTS,
                              outstr, sizeof(outstr), &outstrlen,
                              SQL_DRIVER_COMPLETE);

   if ((retcode != SQL_SUCCESS) && (retcode != SQL_SUCCESS_WITH_INFO))
   {
      printf("SQLConnect failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   retcode = bcp_init(hdbc, table_name_par, NULL, NULL, DB_IN);

   if ((retcode != SUCCEED))
   {
      printf("bcp_init Failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   column_holders = bind_columns(hdbc, table_name_par, &column_num);
   if (column_holders == NULL)
   {
      printf("Column binding error! Exiting.\n");
      return EXIT_FAILURE;
   }

   printf("%d \n", column_num);

   int i;
   char c;

   csv_init(&p, CSV_EMPTY_IS_NULL);
   csv_set_delim(&p, delimeter);

   while ((i = getc(stdin)) != EOF)
   {
      if (!allow_send_row_failures && row_write_errors > 0)
         break;
      c = i;
      if (csv_parse(&p, &c, 1, parser_column_callback, parser_row_callback, NULL) != 1)
      {
         fprintf(stderr, "Parser error: %s\n", csv_strerror(csv_error(&p)));
         Cleanup();
         return EXIT_FAILURE;
      }
   }

   csv_fini(&p, parser_column_callback, parser_row_callback, NULL);

   rows_done = bcp_done(hdbc);
   if ((rows_done == -1))
   {
      printf("Bulk copy was failed\n\n");
      Cleanup();
      return EXIT_FAILURE;
   }

   if (row_write_errors > 0)
   {
      printf("Error!. %d rows were not copied. Exiting.\n\n", row_write_errors);
      Cleanup();
      return EXIT_FAILURE;
   }

   printf("Number of rows bulk copied after last bcp_batch call = %d.\n", rows_done);

   Cleanup();

   return EXIT_SUCCESS;
}
