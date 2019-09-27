l10n.defineLocalization('core', {
    'taskDetailsWindowTitle': 'Детали задачи',
    'changeRoleWindowTitle': 'Смена роли',
    'createWindowTitle': 'Новая запись',
    'detailWindowTitle': 'Просмотр записи',
    'updateWindowTitle': 'Редактирование записи',
    'deleteWindowTitle': 'Удаление записи',
    'selectorWindowTitle': 'Форма выбора записи',
    'reviewWindowTitle': 'Форма просмотра',
    'extractParamsWindowTitle': 'Параметры извлечения',
    'processParamsWindowTitle': 'Параметры разбора',
    'importResultFilesWindowTitle': 'Результаты импорта',
    'uploadFileWindowTitle': 'Загрузка файла',
    'uploadingFileWaitMessageText': 'Загрузка',
    'uploadFileLabelText': 'Файл',
    'actualImportWindowTitle': 'Текущий импорт',
    'errorImportFormMessage': 'Необходимо выбрать одно или несколько ограничений',
    'errorNoSelection': 'Необходимо выбрать запись',
    'errorTitle': 'Ошибка',
    'confirmTitle': 'Подтверждение',
    'alertTitle': 'Сообщение',
    'defaultRoleButtonText': '[Роль не выбрана]',
    'defaultBackButtonText': 'Назад',
    'loadingText': 'Загрузка...',
    'savingText': 'Сохранение...',
    'deletingText': 'Удаление...',
    'deleteConfirmMessage': 'Вы действительно хотите удалить запись?',
    'exitButtonText': 'Выход',
    'exitConfirmMessage': 'Вы действительно хотите выйти из системы?',
    'layoutButtonText': 'Расположение элементов',
    'menuHistoryTitle': 'Последние использованные',
    'datetimePickerTitle': 'Дата и время',
    'datetimePickerNowBtnText': 'Сейчас',
    'notSupportedMessage': 'Данная функция не доступна в текущей системе',
    'defaultRoleConfirmMessage': 'Вы действительно хотите назначить эту роль по умолчанию?',
    'filterBarClearButtonText': 'Очистить',
    'taskEmptyText': 'Для данной задачи нет элементов для отображения',
    'editingModeTitle': 'Режим редактирования',
    'cancelEditMsg': 'При выходе из режима редактирования все несохраненные изменения будут утеряны. Продолжить?',
    'passwordChangingWindowTitle': 'Изменение пароля',
    'colorEmptyText': 'Цвет не выбран',
    'noDataInRdFileMessage': 'В RD файле отсутствуют данные для построения графика',
    'closeChartConfirmMessage': 'При закрытии окна текущие данные не сохранятся. Хотите продолжить?',
    'viewTextDataWindowTitle': 'Подробно',
    'Filter': 'Фильтр',
    'BigFilterDisplay': 'Сложный фильтр',
    'SessionExpiredWindowTitle': 'Session is expired', 
    'SessionExpiredMessage': 'Application will be reloaded',

    'customValidators': {
        'StartDateFinishDateOrder': 'Дата начала должна быть меньше даты окончания',
        'UploadFileFormat': 'Формат файла не поддерживается',
        'invalidSearchFieldValue': 'Запись удалена или не создана',
        'EmailValidationError': 'Введён некорректный адрес электронной почты'
    },

    'booleanValues': {
        'true': 'Да',
        'false': 'Нет'
    },

    'buttons': {
        'ok': 'Применить',
        'cancel': 'Отменить',
        'open': 'Открыть',
        'close': 'Закрыть',
        'exit': 'Выйти',
        'continue': 'Продолжить',
        'delete': 'Удалить',
        'browse': 'Обзор',
        'appoint': 'Назначить',
        'upload': 'Загрузить',
        'download': 'Скачать',
        'edit': 'Редактировать'
    },

    'createWindowButtons': {
        'mixin': '.core.buttons',
        'ok': 'Создать'
    },

    'logIn': {
        'title': 'Вход',
        'login': 'Логин',
        'password': 'Пароль',
        'ok': 'Войти'
    },
    'userwindow': {
        'login': 'Логин: ',
        'role': 'Роль: ',
        'passwordButton': 'Изменить пароль'
    },

    'marsCalendar': {
        'title': 'Mars календарь',
        'year': 'Год',
        'period': 'Период',
        'week': 'Неделя',
        'day': 'День',
        'buttons': {
            'mixin': ['.core.buttons'],
            'now': 'Сейчас'
        },
        'errors': {
            'invalidFormat': 'Неверный формат марс-даты',
            'weekRequired': 'Номер недели обязателен для заполнения'
        }
    },

    'charView': {
        'weightAxesTitle': 'Изменение веса, г',
        'timeAxesTitle': 'Дата/время',
    },

    'gridInfoToolbar': {
        'gridInfoMsg': 'Всего записей: {0}',
        'emptyFilter': 'Фильтр не применен',
        'clearFilter': 'Очистить фильтр'
    },

    'additionalMenu': {
        'additionalBtn': 'Дополнительно',
        'importExportBtn': 'Импорт / Экспорт',
        'gridSettingsMenuItem': 'Настроить таблицу',
        'taskDetailsMenuItem': 'Детали задачи',
        'restartMenuItem': 'Перезапуск',
        'startMenuItem': 'Запуск',
        'applyImport': 'Применить импорт',
        'importCSV': 'Импорт CSV',
        'importXLSX': 'Импорт XLSX',
        'fullImportCSV': 'Полный импорт CSV',
        'fullImportXLSX': 'Полный импорт XLSX',
        'importTemplateCSV': 'Шаблон импорта CSV',
        'importTemplateXLSX': 'Шаблон импорта XLSX',
        'exportCSV': 'Экспорт в CSV',
        'exportXLSX': 'Экспорт в XLSX',
        'manualProcess': 'Разобрать',
        'manualSend': 'Отправить',
        'manualDownload': 'Скачать',
        'readLog': 'Лог',
        'importResults': 'Результаты импорта'
    },

    'selectablePanelButtons': {
        'detail': 'Подробно',
        'table': 'Таблица',
        'update': 'Обновить',
        'collapse': 'Свернуть / Развернуть',
        'close': 'Закрыть',
        'toolbarCollapse': 'Свернуть',
        'prev': 'Предыдущая',
        'next': 'Следующая',
        'download': 'Скачать',
    },

    'mainmenu': {
        'DirectoriesMenu': 'Справочники',
        'InterfaceItem': 'Интерфейсы',
        'FileBufferItem': 'Файлы',
        'FileCollectInterfaceSettingItem': 'Настройки сбора файлов',
        'FileSendInterfaceSettingItem': 'Настройки отправки файлов',
        'CSVProcessInterfaceSettingItem': 'Настройки входящих CSV-интерфейсов',
        'XMLProcessInterfaceSettingItem': 'Настройки входящих XML-интерфейсов',
        'CSVExtractInterfaceSettingItem': 'Настройки исходящих CSV-интерфейсов',
        'directoriesMenu': 'Справочники',
        'adminMenu': 'Администрирование',
        'loopHandlerItem': 'Обработчики',
        'singleLoopHandlerItem': 'Задачи',
        'UserItem': 'Пользователи',
        'RoleItem': 'Роли',
        'AccessPointItem': 'Точки доступа',
        'ConstraintItem': 'Ограничения',
        'SettingItem': 'Настройки',
        'MailNotificationSettingItem': 'Настройки почтовых уведомлений',
        'SystemDirectories': 'Системные справочники'
    },

    'compositePanelTitles': {
        'InterfaceTitle': 'Интерфейсы',
        'FileCollectInterfaceSettingTitle': 'Настройки сбора файлов',
        'FileSendInterfaceSettingTitle': 'Настройки отправки файлов',
        'FileBufferTitle': 'Файлы',
        'CSVProcessInterfaceSettingTitle': 'Настройки входящих CSV-интерфейсов',
        'XMLProcessInterfaceSettingTitle': 'Настройки входящих XML-интерфейсов',
        'CSVExtractInterfaceSettingTitle': 'Настройки исходящих CSV-интерфейсов',
        'historyPanelTitle': 'История',
        'deletedPanelTitle': 'Удалённые',
        'LoopHandlerTitle': 'Обработчики',
        'UserLoopHandlerTitle': 'Задачи',
        'SingleLoopHandlerTitle': 'Задачи',
        'AssociatedUserTitle': 'Пользователи',
        'AdUserTitle': 'Пользователи из Active Directory',
        'RoleTitle': 'Роли',
        'AssociatedAccessPointTitle': 'Точки доступа',
        'AssociatedUserRoleTitle': 'Роли назначенные пользователю',
        'AssociatedAccessPointRoleTitle': 'Роли назначенные точке доступа',
        'ConstraintTitle': 'Ограничения',
        'UserRoleMainTitle': 'Пользователи и роли',
        'SettingTitle': 'Настройки',
        'MailNotificationSettingTitle': 'Настройки почтовых уведомлений',
        'AssociatedMailNotificationSettingTitle': 'Настройки почтовых уведомлений',
        'RecipientTitle': 'Получатели',
        'AssociatedRecipientTitle': 'Получатели'
    },

    'crud': {
        'createButtonText': 'Создать',
        'updateButtonText': 'Изменить',
        'deleteButtonText': 'Удалить',
        'historyButtonText': 'История',
        'addButtonText': 'Добавить'
    },

    'toptoolbar': {
        'deletedButtonText': 'Удалённые',
        'filterButtonText': 'Расширенный фильтр'
    },

    'filter': {
        'filterTitle': 'Расширенный фильтр',
        'taskOnly': 'Задачи',
        'reportsOnly': 'Отчеты',
        'filterSettingsTitle': 'Выбор полей фильтра',
        'selectionFilter': 'Стандартный вид',
        'textFilter': 'Текстовое представление',
        'fieldCheckboxLabel': 'Поле',
        'labelFrom': 'С',
        'labelTo': 'по',
        'all': 'Все',
        'settingsHeader': 'Имя поля',
        'clearConfirmMessage': 'При выполнении данной операции фильтр будет сброшен. Хотите продолжить?',
        'settingsErrorMessage': 'Выберите хотя бы одно поле!',
        'filterErrorMessage': 'В форме есть некорректные значения.',
        'filterEmptyStatus': 'Фильтр не применен',
        'filterNotEmptyStatus': 'Фильтр применен',
        'buttons': {
            'settings': 'Выбрать поля фильтра',
            'filtertype': 'Выбрать тип фильтра',
            'apply': 'Применить',
            'reject': 'Очистить',
            'close': 'Отменить'
        },
        'operations': {
            'Equals': 'Равно',
            'NotEqual': 'Не равно',
            'GraterThan': 'Больше',
            'GraterOrEqual': 'Больше или равно',
            'LessThan': 'Меньше',
            'LessOrEqual': 'Меньше или равно',
            'Between': 'Диапазон',
            'In': 'Список',
            'IsNull': 'Пусто',
            'NotNull': 'Не пусто',
            'Contains': 'Содержит',
            'NotContains': 'Не содержит',
            'StartsWith': 'Начинается с',
            'EndsWith': 'Заканчивается на'
        }
    },

    'multiSelect': {
        'windowTitle': 'Множественный выбор',
        'multiValueButtonText': 'Список значений',
        'multiRangeButtonText': 'Список диапазонов',
        'emptyText': '[пусто]',
        'buttons': {
            'add': 'Добавить',
            'delete': 'Удалить'
        }
    },

    'gridsettings': {
        'gridSettingsTitle': 'Настройка таблицы',
        'hiddenСolumnsTitle': 'Скрытые колонки',
        'visibleСolumnsTitle': 'Видимые колонки',
        'widthFieldLabel': 'Ширина',
        'selectSortMsg': 'Сортировка уже применена к одной из колонок. Хотите переназначить?',
        'emptyWidthText': 'авто',
        'columns': {
            'Name': 'Имя',
            'Width': 'Ширина',
            'Sort': 'Сортировка'
        },
        'buttons': {
            'add': 'Добавить',
            'remove': 'Удалить',
            'up': 'Вверх',
            'down': 'Вниз',
            'sortAsc': 'Сортировка по возрастанию',
            'sortDesc': 'Сортировка по убыванию',
            'clearSort': 'Без сортировки'
        },
        'windowButtons': {
            'save': 'Сохранить',
            'applydefault': 'По умолчанию',
            'cancel': 'Отменить'
        }
    },

    'enums': {
        'OperationType': {
            'Created': 'Создание',
            'Disabled': 'Удаление',
            'Deleted': 'Удаление',
            'Updated': 'Изменение'
        },
        'SortMode': {
            'ASC': 'По возрастанию',
            'DESC': 'По убыванию'
        }
    },

    'BaseHistoryEntity': {
        'OperationType': ['.core.enums.OperationType'],
        '_User': 'Пользователь',
        '_Role': 'Роль',
        '_EditDate': 'Дата/время изменения',
        '_Operation': 'Действие'
    },

    'BaseDeletedEntity': {
        'DeletedDate': 'Дата/время удаления'
    },

    'BaseImportEntity': {
        'HasErrors': 'Есть ошибки',
        'ErrorMessage': 'Ошибка'
    },

    // Localisation for directories

    'LoopHandler': {
        'Description': 'Описание',
        'Name': 'Название',
        'ExecutionMode': 'Режим запуска',
        'Status': 'Статус',
        'ExecutionPeriod': 'Периодичность запуска',
        'CreateDate': 'Дата создания',
        'LastExecutionDate': 'Дата последнего запуска',
        'NextExecutionDate': 'Дата следующего запуска',
        'ConfigurationName': 'Конфигурация',
        'UserId': '',
        'UserName': 'Пользователь',
        'ParametersTitle': 'Параметры',
        'ResultTitle': 'Результат',
        'TaskTitle': 'Задача',
		'ReadLogTitle': 'Лог',
		'CreateMessage': 'Задача обработчика успешно создана.',
		'LogWindowError': 'Ошибка при отображении окна лога.',
		'ManualDataExtractionTask': 'Задача ручного извлечения данных успешно создана.',
		'NoSelectionMessage': 'Сначала нужно выделить запись.',
		'NoSelectionTaskMessage': 'Сначала нужно выделить задачу.',
		'ParseTaskMessage': 'Задача разбора сообщения успешно создана.',
		'SendMsgTaskMessage': 'Задача отправки сообщения успешно создана.',
        'ParameterNames': {
            'File': 'Файл',
            'FileModel': 'Файл экспорта',
            'CrossParam.ClearTable': 'Очистить данные',
            'InterfaceName': 'Интерфейс',
            'FileList': 'Файлы',
            'ManualCollect': 'Запустить вручную',
            'ManualProcess': 'Запустить вручную',
            'ManualExtract': 'Запустить вручную',
            'ExportSourceRecordCount': 'Исходных записей',
            'ExportResultRecordCount': 'Записей обработано',
            'ImportSubtypeName': 'Подтип импорта',
            'ImportTypeDisplay': 'Справочник',
            'ImportResult': 'Загрузка',
            'ImportResultFilesModel': 'Результаты импорта',
            'ImportSourceRecordCount': 'Исходных записей',
            'ImportResultRecordCount': 'Записей обработано',
            'ErrorCount': 'Количество ошибок',
            'WarningCount': 'Количество предупреждений',
            'Materials': 'Материалы',
            'Infinities': 'Инфинити',
            'ResultMessage': 'Результат',
            'NoValidFile': 'Ошибки'
        }
    },
    'HistoricalLoopHandler': ['LoopHandler', 'BaseHistoryEntity'],

    'UserLoopHandler': {
        'Description': 'Описание',
        'Name': 'Название',
        'ExecutionMode': 'Режим запуска',
        'Status': 'Статус',
        'CreateDate': 'Дата создания',
        'ExecutionPeriod': 'Периодичность запуска',
        'LastExecutionDate': 'Дата последнего запуска',
        'NextExecutionDate': 'Дата следующего запуска',
        'ConfigurationName': 'Конфигурация',
        'UserId': '',
        'UserName': 'Пользователь'
    },

    'FileBuffer': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'InterfaceDirection': 'Направление',
        'CreateDate': 'Дата создания',
        'UserId': '',
        'HandlerId': '',
        'FileName': 'Файл',
        'FileData': '',
        'Status': 'Статус',
        'ProcessDate': 'Дата обработки',
        'FileDataTitle': 'Содержимое',
        'ImportResultFileTypes': {
            'Success': 'Импортировано',
            'Warning': 'Предупреждения',
            'Error': 'Ошибки'
        }
    },
    'HistoricalFileBuffer': ['FileBuffer', 'BaseHistoryEntity'],

    'FileCollectInterfaceSetting': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'SourcePath': 'Исходный каталог',
        'SourceFileMask': 'Шаблон названия исходного файла',
        'CollectHandler': 'Обработчик сбора'
    },
    'HistoricalFileCollectInterfaceSetting': ['FileCollectInterfaceSetting', 'BaseHistoryEntity'],

    'FileSendInterfaceSetting': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'TargetPath': 'Каталог назначения',
        'TargetFileMask': 'Шаблон названия исходящего файла',
        'SendHandler': 'Обработчик отправки'
    },
    'HistoricalFileSendInterfaceSetting': ['FileSendInterfaceSetting', 'BaseHistoryEntity'],

    'CSVProcessInterfaceSetting': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'Delimiter': 'Разделитель',
        'UseQuoting': 'Использование кавычек',
        'QuoteChar': 'Символ кавычки',
        'ProcessHandler': 'Обработчик разбора'
    },
    'HistoricalCSVProcessInterfaceSetting': ['CSVProcessInterfaceSetting', 'BaseHistoryEntity'],

    'XMLProcessInterfaceSetting': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'RootElement': 'Корневой элемент',
        'ProcessHandler': 'Обработчик разбора'
    },
    'HistoricalXMLProcessInterfaceSetting': ['XMLProcessInterfaceSetting', 'BaseHistoryEntity'],

    'CSVExtractInterfaceSetting': {
        'InterfaceId': 'Интерфейс',
        'InterfaceName': 'Интерфейс',
        'FileNameMask': 'Шаблон названия файла',
        'ExtractHandler': 'Обработчик выгрузки'
    },
    'HistoricalCSVExtractInterfaceSetting': ['CSVExtractInterfaceSetting', 'BaseHistoryEntity'],

    'Interface': {
        'Name': 'Наименование',
        'Direction': 'Направление',
        'Description': 'Описание'
    },
    'HistoricalInterface': ['Interface', 'BaseHistoryEntity'],

    'SingleLoopHandler': {
        'Description': 'Описание',
        'Name': 'Название',
        'ExecutionMode': 'Режим запуска',
        'Status': 'Статус',
        'CreateDate': 'Дата создания',
        'ExecutionPeriod': 'Периодичность запуска',
        'LastExecutionDate': 'Дата последнего запуска',
        'NextExecutionDate': 'Дата следующего запуска',
        'ConfigurationName': 'Конфигурация',
        'UserId': '',
        'UserName': 'Пользователь'
    },

    'User': {
        'Sid': 'Sid',
        'Name': 'Логин',
        'Password': 'Пароль',
        'Email': 'Электронная почта'
    },
    'HistoricalUser': ['User', 'BaseHistoryEntity'],
    'DeletedUser': ['User', 'BaseDeletedEntity'],
    'AssociatedUser': {
        'mixin': '.core.User',
        'buttons': {
            'changePassButtonText': 'Изменить пароль'
        }
    },
    'HistoricalAssociatedUser': ['AssociatedUser', 'BaseHistoryEntity'],
    'DeletedAssociatedUser': ['AssociatedUser', 'BaseDeletedEntity'],

    'AdUser': {
        'Sid': 'Sid',
        'Name': 'Логин',
        'Email': 'Электронная почта'
    },

    'Role': {
        'SystemName': 'Системное имя',
        'DisplayName': 'Роль',
        'IsAllow': 'Доступность'
    },
    'HistoricalRole': ['Role', 'BaseHistoryEntity'],
    'DeletedRole': ['Role', 'BaseDeletedEntity'],

    'AccessPoint': {
        'Resource': 'Ресурс',
        'Action': 'Действие',
        'Description': 'Описание'
    },
    'HistoricalAccessPoint': ['AccessPoint', 'BaseHistoryEntity'],
    'DeletedAccessPoint': ['AccessPoint', 'BaseDeletedEntity'],
    'AssociatedAccessPoint': ['AccessPoint'],
    'HistoricalAssociatedAccessPoint': ['AssociatedAccessPoint', 'BaseHistoryEntity'],
    'DeletedAssociatedAccessPoint': ['AssociatedAccessPoint', 'BaseDeletedEntity'],

    'AccessPointRole': ['Role'],
    'AssociatedAccessPointRole': ['AccessPointRole'],

    'UserRole': {
        'IsDefault': 'По умолчанию',
        'SystemName': 'Системное название роли',
        'DisplayName': 'Роль',
        'IsAllow': 'Доступность роли',
        'buttons': {
            'setdefault': 'Назначить по умолчанию'
        }
    },
    'AssociatedUserRole': ['UserRole'],

    'ConstraintUserRole': {
        'IsDefault': 'По умолчанию',
        'Login': 'Логин пользователя',
        'RoleSystemName': 'Системное название роли',
        'RoleDisplayName': 'Роль',
        'RoleIsAllow': 'Доступность роли'
    },

    'Constraint': {
        'ConstraintPrefixEnum': {
        },
        'Prefix': 'Тип',
        'Value': 'Значение'
    },
    'HistoricalConstraint': ['Constraint', 'BaseHistoryEntity'],
    'DeletedConstraint': ['Constraint', 'BaseDeletedEntity'],

    'Setting': {
        'Name': 'Название',
        'Type': 'Тип',
        'Value': 'Значение',
        'Description': 'Описание'
    },
    'HistoricalSetting': ['Setting', 'BaseHistoryEntity'],
    'DeletedSetting': ['Setting', 'BaseDeletedEntity'],  

    'UserRoleMain': {
        'Login': 'Логин пользователя',
        'DisplayName': 'Роль'
    },

    'MailNotificationSetting': {
        'Id' : '',
        'Name' : 'Название',
        'Description' : 'Описание',
        'Subject' : 'Тема',
        'Body' : 'Содержимое',
        'IsDisabled' : 'Отключено',
        'To' : 'Адреса получателей',
        'CC' : 'Копии',
        'BCC' : 'Скрытые копии',
        'DeletedDate' : 'Дата удаления'
    },
    'HistoricalMailNotificationSetting': ['MailNotificationSetting', 'BaseHistoryEntity'],
    'DeletedMailNotificationSetting': ['MailNotificationSetting', 'BaseDeletedEntity'],
    'AssociatedMailNotificationSetting': ['MailNotificationSetting'],
    'DeletedAssociatedMailNotificationSetting': ['AssociatedMailNotificationSetting', 'BaseDeletedEntity'],
    'HistoricalAssociatedMailNotificationSetting': ['AssociatedMailNotificationSetting', 'BaseHistoryEntity'],

    'Recipient': {
        'Id' : '',
        'MailNotificationSettingId' : '',
        'Type' : 'Тип',
        'Value' : 'Значение',
        'RecipientTypeEnum': {
            'EMAIL': 'Электронная почта',
            'USER': 'Пользователь',
            'ROLE': 'Роль',
            'FUNCTION': 'Функция'
        }
    },
    'HistoricalRecipient': ['Recipient', 'BaseHistoryEntity'],
    'DeletedRecipient': ['Recipient', 'BaseDeletedEntity'],
    'AssociatedRecipient': ['Recipient'],
    'HistoricalAssociatedRecipient': ['AssociatedRecipient', 'BaseHistoryEntity'],

    'DirectoryGrid': {
        'SelectAllToolTip': 'Выбрать все'
    },

    'System': {
        'Tasks': 'Задачи'
    }
});