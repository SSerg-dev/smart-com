l10n.defineLocalization('core', {
    'taskDetailsWindowTitle': 'Task detail',
    'changeRoleWindowTitle': 'Role change',
    'changeLocaleWindowTitle': 'Language change',
    'createWindowTitle': 'New record',
    'detailWindowTitle': 'View record',
    'updateWindowTitle': 'Record editing',
    'deleteWindowTitle': 'Record removing',
    'selectorWindowTitle': 'Record selecting form',
    'reviewWindowTitle': 'View Form',
    'extractParamsWindowTitle': 'Extract parameters',
    'processParamsWindowTitle': 'Process parameters',
	'importResultFilesWindowTitle': 'Import results',
	'dataLakeSyncResultFilesWindow': 'Synchronization results',
    'uploadFileWindowTitle': 'File uploading',
    'uploadingFileWaitMessageText': 'Uploading',
    'uploadFileLabelText': 'File',
    'actualImportWindowTitle': 'Current import',
    'errorImportFormMessage': 'You must select one or more constraints',
    'errorNoSelection': 'No selection',
    'errorTitle': 'Error',
    'confirmTitle': 'Confirm',
    'alertTitle': 'Message',
    'defaultRoleButtonText': '[The role is not selected]',
    'defaultBackButtonText': 'back',
    'loadingText': 'Loading...',
    'savingText': 'Saving...',
    'deletingText': 'Deleting...',
    'deleteConfirmMessage': 'Do you really want to delete the record?',
    'goToPersonCardButtonText': 'Exit to the main page',
    'goToModulesButtonText': 'Modules',
    'exitButtonText': 'Exit',
    'exitConfirmMessage': 'Do you really want to exit from the system?',
    'menuHistoryTitle': 'Last used',
    'datetimePickerTitle': 'Date and time',
    'datetimePickerNowBtnText': 'Now',
    'notSupportedMessage': 'This function is not available on the current system',
    'defaultRoleConfirmMessage': 'Do you really want to mark this role as default?',
    'filterBarClearButtonText': 'Clean',
    'taskEmptyText': 'There are no items to display for this task',
    'editingModeTitle': 'Editing mode',
    'cancelEditMsg': 'All unsaved changes will be lost. Continue?',
    'passwordChangingWindowTitle': 'Password changing',
    'userInfoTitle': 'User Info',
    'viewTextDataWindowTitle': 'Detail',
    'Filter': 'Filter',
    'defaultResponseError': 'Operation failed',
    'defaultResponseSuccess': 'Operation succsees',
    'defaultMaxLenghtText': 'Maximum length exceeded',
    'WrongUserTypeMessage': 'This operation is not available for Windows accounts',
    'colorEmptyText': 'Color is empty',
    'defaultLocaleButtonText': '[Localization is empty]',
    'TaskSuccessRunMessage': 'Processing successfully started',
    'accessDeniedMessage': 'Access denied ({0}). If this error repeat, contact administrator.',
    'loginConfirmMessage': 'You will be redirected to the main page as user {0}.',
    'SessionExpiredWindowTitle': 'Session is expired',
    'SessionExpiredMessage': 'Due to inactivity your session has expired. Application will be reloaded.',
    'SignalRConnectionWasLost': 'Session timed outt.',
    'SignalRConnectionWasLostMessage': 'Session timed out. Application will be reloaded.',

    'customValidators': {
        'StartDateFinishDateOrder': 'Stary date must be less then end date',
        'UploadFileFormat': 'Wrong file format. Required format: {0}',
        'PasswordFormat': 'The password can not be shorter than 5 characters and contains spaces',
        'clientTreeSelectRoot': "Root element couldn't be selected",
        'EmailValidationError': 'Invalid email',
        'invalidSearchFieldValue': 'Such a record no longer exists.'
    },

    'booleanValues': {
        'true': 'Yes',
        'false': 'No'
    },

    'buttons': {
        'ok': 'Apply',
        'cancel': 'Cancel',
        'open': 'Open',
        'close': 'Close',
        'exit': 'Exit',
        'continue': 'Continue',
        'delete': 'Delete',
        'browse': 'Browse',
        'appoint': 'Appoint',
        'upload': 'Upload',
        'download': 'Download',
        'edit': 'Edit',
        'save': 'Save',
        'confirm': 'Confirm'
    },

    'createWindowButtons': {
        'mixin': '.core.buttons',
        'ok': 'Create'
    },

    'logIn': {
        'title': 'Log In',
        'login': 'Login',
        'password': 'Password',
        'ok': 'Log In'
    },
    'userwindow': {
        'login': 'Логин: ',
        'role': 'Роль: ',
        'passwordButton': 'Изменить пароль'
    },

    'marsCalendar': {
        'title': 'Mars Calendar',
        'year': 'Year',
        'period': 'Period',
        'week': 'Week',
        'day': 'Day',
        'buttons': {
            'mixin': ['.core.buttons'],
            'now': 'Now'
        },
        'errors': {
            'invalidFormat': 'Invalid format',
            'weekRequired': 'Week is required'
        }
    },

    'gridInfoToolbar': {
        'gridInfoMsg': 'Records count: {0}',
        'emptyFilter': 'Filter is empty',
        'clearFilter': 'Clear filter',
        'checkedRecordsCount': 'Selected count: {0}'
    },

    'additionalMenu': {
        'additionalBtn': 'Additional',
        'importExportBtn': 'Import / Export',
        'gridSettingsMenuItem': 'Table settings',
        'taskDetailsMenuItem': 'Task detail',
        'restartMenuItem': 'Restart',
        'startMenuItem': 'Start'
    },

    'additionalMenu': {
        'additionalBtn': 'Additional',
        'importExportBtn': 'Import / Export',
        'gridSettingsMenuItem': 'Table settings',
        'taskDetailsMenuItem': 'Task detail',
        'restartMenuItem': 'Restart',
        'startMenuItem': 'Start',
        'applyImport': 'Apply import',
        'importCSV': 'Import CSV',
        'importXLSX': 'Import XLSX',
        'fullImportCSV': 'Full import CSV',
        'fullImportXLSX': 'Full import XLSX',
        'fullImportEANXLSX': 'Full import EAN PC XLSX',
        'fullImportPLUXLSX': 'Full import PLU XLSX',
        'importTemplateCSV': 'Import template CSV',
        'importTemplateXLSX': 'Import template XLSX',
        'importTemplateEANPCXLSX': 'Import template EAN PC XLSX',
        'importTemplatePLUXLSX': 'Import template PLU XLSX',
        'exportCSV': 'Export to CSV',
        'exportXLSX': 'Export to XLSX',
        'manualProcess': 'Parse',
        'manualSend': 'Send',
        'manualDownload': 'Download',
        'readLog': 'Log',
        'importResults': 'Import results',
        'newFullImportXLSX': 'Full import correction XLSX'
    },

    'importExport': {
        'exportXLSXButton': 'Export XLSX',
        'exportXLSXScheduleButton': 'Export XLSX(Background)',
        'exportCSVButton': 'Export CSV',
        'exportCSVScheduleButton': 'Export CSV(Background)',
        'templateXLSX': 'Import template XLSX',
        'templateCSV': 'Import template CSV',
        'fullImportXLSXButton': 'Import XLSX',
        'fullImportCSVButton': 'Import CSV',
        'applyImportButton': 'Apply import'
    },

    'selectablePanelButtons': {
        'detail': 'Detail',
        'table': 'Table',
        'update': 'Update',
        'collapse': 'Collapse / Expand',
        'close': 'Close',
        'toolbarCollapse': 'Collapse',
        'prev': 'Previous',
        'next': 'Next',
        'download': 'Download',
    },

    'mainmenu': {
        'DirectoriesMenu': 'Directories',
        'InterfaceItem': 'Interfaces',
        'FileBufferItem': 'Files',
        'FileCollectInterfaceSettingItem': 'File collect settings',
        'FileSendInterfaceSettingItem': 'File send settings',
        'CSVProcessInterfaceSettingItem': 'Inbound CSV interfaces settings',
        'XMLProcessInterfaceSettingItem': 'Inbound XML interfaces settings',
        'CSVExtractInterfaceSettingItem': 'Outbound CSV interfaces settings',
        'directoriesMenu': 'Directories',
        'adminMenu': 'Administration',
        'loopHandlerItem': 'Handlers',
        'singleLoopHandlerItem': 'Tasks',
        'UserItem': 'Users',
        'RoleItem': 'Roles',
        'AccessPointItem': 'Access Points',
        'ConstraintItem': 'Constraints',
        'SettingItem': 'Settings',
        'MailNotificationSettingItem': 'Mail notification settings',
        'RPASettingItem': 'RPA settings',
        'RPAItem': 'RPA',
        'SystemDirectories': 'Administration',
        'SubscriptionMenu': 'Subscription',
        'EventTypeItem': 'Event types',
        'EventItem': 'Events',
        'SubscriptionItem': 'Subscriptions',
        'SystemMenu': 'Administration',
        'AttendanceType': 'Attendance Types',
        'ModuleItem': 'Modules'
    },

    'compositePanelTitles': {
        'InterfaceTitle': 'Interfaces',
        'FileCollectInterfaceSettingTitle': 'File collect settings',
        'FileSendInterfaceSettingTitle': 'File send settings',
        'FileBufferTitle': 'Files',
        'CSVProcessInterfaceSettingTitle': 'Inbound CSV interfaces settings',
        'XMLProcessInterfaceSettingTitle': 'Inbound XML interfaces settings',
        'CSVExtractInterfaceSettingTitle': 'Outbound CSV interfaces settings',
        'historyPanelTitle': 'History',
        'deletedPanelTitle': 'Deleted',
        'LoopHandlerTitle': 'Handlers',
        'UserLoopHandlerTitle': 'Tasks',
        'SingleLoopHandlerTitle': 'Tasks',
        'AssociatedUserTitle': 'Users',
        'AdUserTitle': 'Users',
        'RoleTitle': 'Roles',
        'AssociatedAccessPointTitle': 'Access points',
        'AssociatedUserRoleTitle': 'User roles',
        'AssociatedAccessPointRoleTitle': 'Roles assigned to the access point',
        'ConstraintTitle': 'Constraints',
        'UserRoleMainTitle': 'Users and Roles',
        'SettingTitle': 'Settings',
        'MailNotificationSettingTitle': 'Mail notification settings',
        'RPASettingTitle': 'RPA settings',
        'RPATitle': 'RPA',
        'AssociatedMailNotificationSettingTitle': 'Mail notification settings',
        'RecipientTitle': 'Recipients',
        'AssociatedRecipientTitle': 'Recipients',
        'ModuleTitle': 'Modules',
        'ModuleRoleTitle': 'Roles',
        'EventTypeTitle': 'Event types',
        'EventTitle': 'Events',
        'AssociatedUserSubscriptionTitle': 'Subscriptions',
        'AttendanceTypeTitle': 'Attendance Types',
        'PersonCardTitle': 'Persons'
    },

    'crud': {
        'createButtonText': 'Create',
        'updateButtonText': 'Edit',
        'deleteButtonText': 'Delete',
        'historyButtonText': 'History',
        'addButtonText': 'Add'
    },

    'toptoolbar': {
        'deletedButtonText': 'Deleted',
        'filterButtonText': 'Extended filter'
    },

    'filter': {
        'filterTitle': 'Extended filter',
        'taskOnly': 'Tasks',
        'reportsOnly': 'Reports',
        'filterSettingsTitle': 'Filter settings',
        'selectionFilter': 'Standard filter',
        'textFilter': 'Text filter',
        'fieldCheckboxLabel': 'Field',
        'labelFrom': 'From',
        'labelTo': 'To',
        'all': 'All',
        'settingsHeader': 'FieldName',
        'clearConfirmMessage': 'Filter will be cleared. Continue?',
        'settingsErrorMessage': 'At least one field must be selected!',
        'filterErrorMessage': 'Form contains incorrect values.',
        'filterEmptyStatus': 'Filter not applied',
        'filterNotEmptyStatus': 'Filter applied',
        'buttons': {
            'settings': 'Filter field choice',
            'filtertype': 'Filter type choice',
            'apply': 'Apply',
            'reject': 'Clear',
            'close': 'Cancel'
        },
        'operations': {
            'Equals': 'Equals',
            'NotEqual': 'Not Equal',
            'GreaterThan': 'Greater Than',
            'GreaterOrEqual': 'Greater Or Equal',
            'LessThan': 'Less Than',
            'LessOrEqual': 'Less Or Equal',
            'Between': 'Between',
            'In': 'List',
            'IsNull': 'Is Null',
            'NotNull': 'Not Null',
            'Contains': 'Contains',
            'NotContains': 'Not Contains',
            'StartsWith': 'Starts With',
            'EndsWith': 'Ends With'
        }
    },

    'multiSelect': {
        'windowTitle': 'Multiselect',
        'multiValueButtonText': 'Values list',
        'multiRangeButtonText': 'Ranges list',
        'emptyText': '[empty]',
        'buttons': {
            'add': 'Add',
            'delete': 'Delete'
        }
    },

    'gridsettings': {
        'gridSettingsTitle': 'Table Settings',
        'hiddenСolumnsTitle': 'Hidden Сolumns',
        'visibleСolumnsTitle': 'Visible Сolumns',
        'widthFieldLabel': 'Width',
        'selectSortMsg': 'Sort already applied to one of the columns. Do you want to reassign?',
        'emptyWidthText': 'auto',
        'columns': {
            'Name': 'Name',
            'Width': 'Width',
            'Sort': 'Sort'
        },
        'buttons': {
            'add': 'Add',
            'remove': 'Remove',
            'up': 'Up',
            'down': 'Down',
            'sortAsc': 'Sort Ascending',
            'sortDesc': 'Sort Descending',
            'clearSort': 'Clear Sort'
        },
        'windowButtons': {
            'save': 'Save',
            'applydefault': 'Default',
            'cancel': 'Cancel'
        }
    },

    'enums': {
        'OperationType': {
            'Created': 'Create',
            'Disabled': 'Delete',
            'Deleted': 'Delete',
            'Updated': 'Edit'
        },
        'SortMode': {
            'ASC': 'Ascending',
            'DESC': 'Descending'
        }
    },

    'BaseHistoryEntity': {
        'OperationType': ['.core.enums.OperationType'],
        '_User': 'User',
        '_Role': 'Role',
        '_EditDate': 'Edit date',
        '_Operation': 'Operation'
    },

    'BaseDeletedEntity': {
        'DeletedDate': 'Deleted date'
    },

    'BaseImportEntity': {
        'HasErrors': 'Has errors',
        'ErrorMessage': 'Error'
    },

    // Localisation for directories

    'LoopHandler': {
        'Description': 'Description',
        'Name': 'Name',
        'ServiceName': 'Service name',
        'ServiceMethodName': 'Method',
        'ExecutionMode': 'Execution mode',
        'Status': 'Status',
        'ExecutionPeriod': 'Execution period',
        'CreateDate': 'Date created',
        'LastExecutionDate': 'Last execution date',
        'NextExecutionDate': 'Next execution date',
        'ConfigurationName': 'Configuration',
        'User': 'User',
        'Role': 'Role',
        'Module': 'Module',
        'UserName': 'User',
        'ParametersTitle': 'Parameters',
        'ResultTitle': 'Result',
        'TaskTitle': 'Task',
		'ReadLogTitle': 'Log',
		'CreateMessage': 'Handler task created successfully.',
		'LogWindowError': 'Error while displaying the log window.',
		'ManualDataExtractionTask': 'The task of manual data extraction successfully created.',
		'NoSelectionMessage': 'First you need to select the record.',
		'NoSelectionTaskMessage': 'First you need to select the task.',
		'ParseTaskMessage': 'Message parsing task successfully created.',
		'SendMsgTaskMessage': 'Message sending task created successfully.',
        'ParameterNames': {
            'File': 'File',
            'fileCount':'File Count',
            'FileModel': 'Export file',
            'CrossParam.ClearTable': 'Clear data',
            'InterfaceName': 'Interface',
            'FileList': 'Files',
            'ManualCollect': 'Manually start',
            'ManualProcess': 'Manually start',
            'ManualExtract': 'Manually start',
            'ExportSourceRecordCount': 'Source records count',
            'ExportResultRecordCount': 'Result records count',
            'ImportTypeDisplay': 'Directory',
            'ImportResult': 'Import Records',
            'ImportResultFilesModel': 'Import result',
            'ImportSourceRecordCount': 'Source record count',
			'ImportResultRecordCount': 'Result record count',
            'DataLakeSyncSourceRecordCount': 'Synchronization result',
            'DataLakeSyncResultRecordCount': 'Sync success count',
			'DataLakeSyncResultFilesModel': 'Result record count',
			'DataLakeSyncTypeDisplay': 'Directory',
			'DataLakeSyncResult': 'Import Records',
            'ErrorCount': 'Errors count',
            'ErrorsToNotify': 'Errors To Notify',
            'WarningCount': 'Warnings count',
            'Materials': 'Materials',
            'Infinities': 'Infinities',
            'ResultMessage': 'Result',
            'NoValidFile': 'Errors',
            'CrossParam.ClientFilter': 'Client Filter',
            'CrossParam.StartDate': ' Start Date',
            'CrossParam.FinishDate': 'Finish Date',
            'CrossParam.Year': 'Year',
            'UniqueFields': 'Unique Fields'
        },
        'DownloadLogTitle': 'Results'
    },
    'HistoricalLoopHandler': ['LoopHandler', 'BaseHistoryEntity'],
    'SingleLoopHandler': ['LoopHandler'],

    'UserLoopHandler': ['LoopHandler'],

    'FileBuffer': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'InterfaceId',
        'InterfaceDirection': 'Direction',
        'CreateDate': 'Date created',
        'UserId': '',
        'HandlerId': '',
        'FileName': 'File',
        'FileData': '',
        'Status': 'Status',
        'ProcessDate': 'Process date',
        'FileDataTitle': 'File data',
		'ImportResultFileTypes': {
			'Success': 'Successes',
			'Warning': 'Warnings',
			'Error': 'Errors'
		},
        'DataLakeSyncResultFileTypes': {
            'Success': 'Successes',
            'Warning': 'Warnings',
            'Error': 'Errors'
        },
    },
    'HistoricalFileBuffer': ['FileBuffer', 'BaseHistoryEntity'],

    'FileCollectInterfaceSetting': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'Interface',
        'SourcePath': 'Source path',
        'SourceFileMask': 'Source file mask',
        'CollectHandler': 'Collect handler'
    },
    'HistoricalFileCollectInterfaceSetting': ['FileCollectInterfaceSetting', 'BaseHistoryEntity'],

    'FileSendInterfaceSetting': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'Interface',
        'TargetPath': 'Target path',
        'TargetFileMask': 'Target file mask',
        'SendHandler': 'Send handler'
    },
    'HistoricalFileSendInterfaceSetting': ['FileSendInterfaceSetting', 'BaseHistoryEntity'],

    'CSVProcessInterfaceSetting': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'Interface',
        'Delimiter': 'Delimiter',
        'UseQuoting': 'Use quoting',
        'QuoteChar': 'Quote char',
        'ProcessHandler': 'Process handler'
    },
    'HistoricalCSVProcessInterfaceSetting': ['CSVProcessInterfaceSetting', 'BaseHistoryEntity'],

    'XMLProcessInterfaceSetting': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'Interface',
        'RootElement': 'Root element',
        'ProcessHandler': 'Process handler'
    },
    'HistoricalXMLProcessInterfaceSetting': ['XMLProcessInterfaceSetting', 'BaseHistoryEntity'],

    'CSVExtractInterfaceSetting': {
        'InterfaceId': 'Interface',
        'InterfaceName': 'Interface',
        'FileNameMask': 'File name mask',
        'ExtractHandler': 'Extract handler'
    },
    'HistoricalCSVExtractInterfaceSetting': ['CSVExtractInterfaceSetting', 'BaseHistoryEntity'],

    'Interface': {
        'Name': 'Name',
        'Direction': 'Direction',
        'Description': 'Description'
    },
    'HistoricalInterface': ['Interface', 'BaseHistoryEntity'],

    'SingleLoopHandler': {
        'Description': 'Description',
        'Name': 'Name',
        'ExecutionMode': 'Execution mode',
        'Status': 'Status',
        'CreateDate': 'Create date',
        'ExecutionPeriod': 'Execution period',
        'LastExecutionDate': 'Last execution date',
        'NextExecutionDate': 'Next execution date',
        'ConfigurationName': 'Configuration',
        'UserId': '',
        'UserName': 'User'
    },

    'User': {
        'Sid': 'Sid',
        'Name': 'Login',
        'Password': 'Password',
        'Email': 'Email почта',
        'ChrisId': 'ChrisId',
        'IsWindows': 'Windows',
        'Login': 'Login',
        'LastName': 'Last Name',
        'FirstName': 'First Name',
        'MiddleName': 'Middle Name',
        'IsLockedOut': 'Is locked out',
        'DefaultLanguage': 'Default language',
        'CreateDate': 'Date created',
        'PasswordExpirationDate': 'Password expiration date',
        'LastAccessFailedDate': 'Last access failed date',
        'AccessFailedCount': 'Access failed count'
    },
    'HistoricalUser': ['User', 'BaseHistoryEntity'],
    'DeletedUser': ['User', 'BaseDeletedEntity'],
    'AssociatedUser': {
        'mixin': '.core.User',
        'buttons': {
            'changePassButtonText': 'Change password',
            'logInButtonText': 'Log in as this user'
        }
    },
    'HistoricalAssociatedUser': ['AssociatedUser', 'BaseHistoryEntity'],
    'DeletedAssociatedUser': ['AssociatedUser', 'BaseDeletedEntity'],

    'AdUser': {
        'Sid': 'Sid',
        'Name': 'Login',
        'Email': 'Email'
    },

    'Role': {
        'SystemName': 'System Name',
        'DisplayName': 'Role',
        'IsAllow': 'IsAllow',
        'ModuleRoleModuleDisplayNameRu': 'Module name Ru',
        'ModuleRoleModuleDisplayNameEn': 'Module name En',
        'ModuleRoleModuleSystemName': 'Module system name',
        'ModuleRoleSystemName': 'Role system name',
        'ModuleRoleDisplayName': 'Module'
    },
    'HistoricalRole': ['Role', 'BaseHistoryEntity'],
    'DeletedRole': ['Role', 'BaseDeletedEntity'],

    'ModuleRole': {
        'SystemName': 'System Name',
        'DisplayName': 'Role',
        'IsAllow': 'Is allow',
        'ModuleDisplayNameRu': 'Module name Ru',
        'ModuleDisplayNameEn': 'Module name En',
        'ModuleSystemName': 'Module system name'
    },
    'DeletedModuleRole': ['ModuleRole', 'BaseDeletedEntity'],

    'UserRole': {
        'IsDefault': 'Is default',
        'SystemName': 'System name',
        'DisplayName': 'Name',
        'IsAllow': 'Is allow',
        'ModuleRoleModuleDisplayNameRu': 'Module name Ru',
        'ModuleRoleModuleDisplayNameEn': 'Module name En',
        'ModuleRoleModuleSystemName': 'Module system name',
        'RoleDisplayName': 'User role',
        'RoleSystemName': 'User role',
        'RoleIsAllow': 'Is allow',
        'buttons': {
            'setdefault': 'Set default'
        }
    },
    'AssociatedUserRole': ['UserRole'],
    'HistoricalAssociatedUserRole': ['AssociatedUserRole', 'BaseHistoryEntity'],
    'DeletedAssociatedUserRole': ['AssociatedUserRole', 'BaseHistoryEntity'], //удаленные записи берутся из истории

    'ConstraintUserRole': {
        'IsDefault': 'Is default',
        'Login': 'Login',
        'RoleSystemName': 'System name',
        'RoleDisplayName': 'Name',
        'RoleIsAllow': 'Is allow',
    },

    'UserModuleRole': {
        'IsDefault': 'Is default',
        'SystemName': 'System name',
        'DisplayName': 'Name',
        'ModuleRoleModuleDisplayNameRu': 'Module name Ru',
        'ModuleRoleModuleDisplayNameEn': 'Module name En',
        'ModuleRoleModuleSystemName': 'Module system name',
        'IsAllow': 'Is allow',
        'buttons': {
            'setdefault': 'Set default'
        }
    },
    'AssociatedUserModuleRole': ['UserModuleRole'],

    'AccessPoint': {
        'Resource': 'Resource',
        'Action': 'Action',
        'Description': 'Description'
    },
    'HistoricalAccessPoint': ['AccessPoint', 'BaseHistoryEntity'],
    'DeletedAccessPoint': ['AccessPoint', 'BaseDeletedEntity'],
    'AssociatedAccessPoint': ['AccessPoint'],
    'HistoricalAssociatedAccessPoint': ['AssociatedAccessPoint', 'BaseHistoryEntity'],
    'DeletedAssociatedAccessPoint': ['AssociatedAccessPoint', 'BaseDeletedEntity'],

    'AccessPointRole': ['Role'],
    'AssociatedAccessPointRole': ['AccessPointRole'],

    'Constraint': {
        'ConstraintPrefixEnum': {
        },
        'Prefix': 'Type',
        'Value': 'Value'
    },
    'HistoricalConstraint': ['Constraint', 'BaseHistoryEntity'],
    'DeletedConstraint': ['Constraint', 'BaseDeletedEntity'],

    'Setting': {
        'Name': 'Name',
        'Type': 'Type',
        'Value': 'Value',
        'Description': 'Description'
    },
    'HistoricalSetting': ['Setting', 'BaseHistoryEntity'],
    'DeletedSetting': ['Setting', 'BaseDeletedEntity'],

    'Module': {
        'SystemName': 'System name',
        'DisplayNameRu': 'Name Ru',
        'DisplayNameEn': 'Name En',
        'DescriptionRu': 'Description Ru',
        'DescriptionEn': 'Description En',
        'Hidden': 'Hidden',
    },
    'DeletedModule': ['Module', 'BaseDeletedEntity'],

    'UserRoleMain': {
        'Login': 'Login',
        'DisplayName': 'Role'
    },

    'MailNotificationSetting': {
        'Id' : '',
        'Name': 'Name',
        'Description': 'Description',
        'Subject': 'Subject',
        'Body': 'Body',
        'IsDisabled': 'Is disabled',
        'To': 'To',
        'CC': 'CC',
        'BCC': 'BCC',
        'DeletedDate': 'Deleted date'
    },
    'HistoricalMailNotificationSetting': ['MailNotificationSetting', 'BaseHistoryEntity'],
    'DeletedMailNotificationSetting': ['MailNotificationSetting', 'BaseDeletedEntity'],
    'AssociatedMailNotificationSetting': ['MailNotificationSetting'],
    'DeletedAssociatedMailNotificationSetting': ['AssociatedMailNotificationSetting', 'BaseDeletedEntity'],
    'HistoricalAssociatedMailNotificationSetting': ['AssociatedMailNotificationSetting', 'BaseHistoryEntity'],

    'RPA': {
        'HandlerName': 'Handler name',
        'CreateDate': 'Create date',
        'UserName': 'User name',
        'Constraint': 'Constraint',
        'Parametrs': 'Parametrs',
        'Status': 'Status',
        'FileURL': 'File URL',
        'LogURL': 'Log URL'
    },

    'RPASetting': {
        'Json': 'Json',
        'Name': 'Name'
    },

    'Recipient': {
        'Id' : '',
        'MailNotificationSettingId' : '',
        'Type': 'Type',
        'Value': 'Value',
        'RecipientTypeEnum': {
            'EMAIL': 'Email',
            'USER': 'User',
            'ROLE': 'Role',
            'FUNCTION': 'Function'
        }
    },
    'HistoricalRecipient': ['Recipient', 'BaseHistoryEntity'],
    'DeletedRecipient': ['Recipient', 'BaseDeletedEntity'],
    'AssociatedRecipient': ['Recipient'],
    'HistoricalAssociatedRecipient': ['AssociatedRecipient', 'BaseHistoryEntity'],

    'EventType': {
        'Name': 'Name',
        'Description': 'Description',
        'ModuleSystemName': 'Module system name',
      'ModuleDisplayName': 'Module name',
      'EmailNotification': 'E-mail Notifications',
      'NotifyImmediately': 'Notify Immediately'
    },
    'Event': {
        'datetime': 'Date',
        'module': 'Module',
        'messagetype': 'Type',
        'messageRu': 'Message in Russian',
        'messageEn': 'Message in English'
    },
    'Subscription': {
        'EventTypeName': 'Name',
        'EventTypeDescription': 'Description',
        'EventTypeModuleDisplayNameRu': 'Module name Ru',
        'EventTypeModuleDisplayNameEn': 'Module name En',
        'EventTypeModuleSystemName': 'Module system name',
        'IsActive': 'Is active',
        'IsRequired': 'Is required',
    },
    'PersonCard': {
        'PersonChrisId': 'Chris ID',
        'PersonLastName_RU': 'Last Name',
        'PersonFirstName_RU': 'Name',
        'PersonMiddleName_RU': 'Middle Name',
        'PersonType': 'Type',
        'PositionDescription_EN': 'Position',
        'UnitDescription': 'Unit',
        'CorpCellPhoneNumber': 'Cell phone',
        'CorpInnerPhoneNumber': 'Inner phone',
        'Email': 'E-mail'
    },
    'AttendanceType': {
        'Type': 'Type',
        'Name': 'Name',
        'Code': 'Code',
        'TimeTableMark': 'TimeTable mark',
        'InputType': 'Input type',
        'Document': 'Document',
        'NeedVerify': 'Need verify',
        'VacationImpact': 'Vacation impact',
        'NumberCode': 'Number code',
        'SickList': 'Sick list',
        'ManualHRCreate': 'Manual create',
        'ExportToFMS': 'Export to FMS',
        'ExportToPayroll': 'Export to Boss',
        'CanComment': 'Can comment'
    },
    'HistoricalAttendanceType': ['AttendanceType', 'BaseHistoryEntity'],
    'DeletedAttendanceType': ['AttendanceType', 'BaseDeletedEntity'],

    'DirectoryGrid': {
        'SelectAllToolTip': 'Select all'
    },

    'System': {
        'Tasks': 'Tasks'
    },

    'HandlerLogFileDownload': {
        'Info': 'Infos',
        'Warning': 'Warnings',
        'Error': 'Errors',
        'Title': 'Results'
    }
});