Ext.define('App.controller.core.loophandler.LoopHandler', {
	extend: 'App.controller.core.CombinedDirectory',
	mixins: ['App.controller.core.ImportExportLogic'],

	init: function () {
		this.listen({
			component: {
				'loophandler[isSearch!=true] directorygrid': {
					load: this.onGridStoreLoad,
					itemdblclick: this.switchToDetailForm
				},
				'loophandler directorygrid': {
					selectionchange: this.onGridSelectionChange,
					afterrender: this.onGridAfterrender,
					extfilterchange: this.onExtFilterChange
				},
				'loophandler #datatable': {
					activate: this.onActivateCard
				},
				'loophandler #detailform': {
					activate: this.onActivateCard
				},
				'loophandler #detailform #prev': {
					click: this.onPrevButtonClick
				},
				'loophandler #detailform #next': {
					click: this.onNextButtonClick
				},
				'loophandler #detail': {
					click: this.switchToDetailForm
				},
				'loophandler #table': {
					click: this.onTableButtonClick
				},
				'loophandler #extfilterbutton': {
					click: this.onFilterButtonClick
				},
				'loophandler #deletedbutton': {
					click: this.onDeletedButtonClick
				},
				'loophandler #createbutton': {
					click: this.onCreateButtonClick
				},
				'loophandler #updatebutton': {
					click: this.onUpdateButtonClick
				},
				'loophandler #deletebutton': {
					click: this.onDeleteButtonClick
				},
				'loophandler #historybutton': {
					click: this.onHistoryButtonClick
				},
				'loophandler #refresh': {
					click: this.onRefreshButtonClick
				},
				'loophandler #close': {
					click: this.onCloseButtonClick
				},
				// import/export
				'loophandler #exportbutton': {
					click: this.onExportButtonClick
				},
				'loophandler #loadimportbutton': {
					click: this.onShowImportFormButtonClick
				},
				'loophandler #loadimporttemplatecsvbutton': {
					click: this.onLoadImportTemplateCSVButtonClick
				},
				'loophandler #loadimporttemplatexlsxbutton': {
					click: this.onLoadImportTemplateXLSXButtonClick
				},
				'loophandler #applyimportbutton': {
					click: this.onApplyImportButtonClick
				},
				'loophandler start': {
					click: this.onStartButtonClick
				},
				'loophandler stop': {
					click: this.onStopButtonClick
				},
				'#taskdetails': {
					click: this.onTaskDetailsButtonClick
				},
				'#manualprocess': {
					click: this.onInterfaceManualProcessButtonClick
				},
				'#manualsend': {
					click: this.onInterfaceManualSendButtonClick
				},
				'#getfiledata': {
					click: this.onGetFileDataButtonClick
				},
				'#downloadbufferfile': {
					click: this.onDownloadFileDataButtonClick
				},
				'#readfilebufferlog': {
					click: this.onReadFileBufferLogButtonClick
				},
				'#importresultfiles': {
					click: this.onImportResultFilesButtonClick
				},
				'editorwindow[name=processparamswindow] #apply': {
					click: this.onProcessApplyButtonClick
				},
				'editorwindow[name=processparamswindow] #close': {
					click: this.onCancelButtonClick
				},
				'editorwindow[name=extractparamswindow] #apply': {
					click: this.onExtractApplyButtonClick
				},
				'editorwindow[name=extractparamswindow] #close': {
					change: this.onEditorWindowCloseButtonClick
				},
				'editorwindow[name=extractparamswindow] importparamform field[isConstrain=true]': {
					change: this.onParamFieldChange
				},
				'loophandlerdatawindow': {
					resize: this.onWindowResize
				},
				'#loophandlerrestart': {
					click: this.onRestartButtonClick
				},
				'#loophandlerstart': {
					click: this.onStartJobButtonClick
				},
				'#loophandlerlog': {
					click: this.onReadLogButtonClick
				}
			}
		});
	},

	onParamFieldChange: function (field, newValue) {
		var form = field.up('importparamform');

		if (!Ext.isEmpty(newValue)) {
			form.removeCls('error-import-form');
			form.down('#errormsg').getEl().hide();
		}
	},

	onEditorWindowCloseButtonClick: function (button) {
		var editor = button.up('window');
		editor.close();
	},

	onGetFileDataButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button);
		var selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			var resource = button.resource || 'FileBuffers';
			var action = button.action || 'FileData';

			grid.setLoading(true);
			breeze.EntityQuery
				.from(resource)
				.withParameters({
					$actionName: action,
					$method: 'POST',
					$entity: record.getProxy().getBreezeEntityByRecord(record)
				})
				.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
				.execute()
				.then(function (data) {
					grid.setLoading(false);
					var resultData = data.httpResponse.data.value;
					me.openTextWindow(resultData, 'viewfiledatawindow');
				})
				.fail(function (data) {
					grid.setLoading(false);
					App.Notify.pushError(me.getErrorMessage(data));
				});
		}
	},

	onDownloadFileDataButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button);
		var selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			var resource = button.resource || 'File';
			var action = button.action || 'DownloadBufferFile';
			var href = document.location.href + Ext.String.format('/api/File/{0}?{1}={2}', action, 'id', record.get('Id'));
			var aLink = document.createElement('a');
			aLink.download = "FileData";
			aLink.href = href;
			document.body.appendChild(aLink);
			aLink.click();
			document.body.removeChild(aLink)
		}
	},

	onInterfaceManualSendButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button),
			selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			if (record.get('InterfaceDirection') !== 'OUTBOUND') {
				App.Notify.pushInfo('The interface is not outgoing');
			} else {
				var resource = button.resource || 'FileBuffers';
				var action = button.action || 'ManualSend';
				//var handlerName = grid.interfaceData ? grid.interfaceData.HandlerName : null;
				grid.setLoading(true);
				breeze.EntityQuery
					.from(resource)
					.withParameters({
						$actionName: action,
						$method: 'POST',
						Id: breeze.DataType.Guid.fmtOData(record.get('Id')),
						CreateDate: breeze.DataType.DateTimeOffset.fmtOData(record.get('CreateDate')),
						InterfaceId: breeze.DataType.Guid.fmtOData(record.get('InterfaceId')),
						InterfaceName: breeze.DataType.String.fmtOData(record.get('InterfaceName'))
					})
					.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
					.execute()
					.then(function (data) {
						grid.setLoading(false);
						var result = Ext.JSON.decode(data.httpResponse.data.value);
						if (result.success) {
							App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('SendMsgTaskMessage'));
							// Открыть панель задач
							me.openUserTasksPanel();
						} else {
							App.Notify.pushError(result.message);
						}
					})
					.fail(function (data) {
						grid.setLoading(false);
						App.Notify.pushError(me.getErrorMessage(data));
					});
			}
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionMessage'));
		}
	},

	getManualProcessParameterForm: function (interfaceName) {
		var result = null;
		if (interfaceName === 'ACT_SALES_ATLAS') {
			result = 'actualsalesinterfaceprocessparamform';
		} else if (interfaceName === 'ACT_SALES_BIOS') {
			result = 'actualsalesinterfaceprocessparamform';
		}
		return result;
	},

	executeManualProcess: function (controller, grid, resource, action, record, parameters, win) {
		grid.setLoading(true);
		var breezeParams = {
			$actionName: action,
			$method: 'POST',
			Id: breeze.DataType.Guid.fmtOData(record.get('Id')),
			CreateDate: breeze.DataType.DateTimeOffset.fmtOData(record.get('CreateDate')),
			InterfaceId: breeze.DataType.Guid.fmtOData(record.get('InterfaceId')),
			InterfaceName: breeze.DataType.String.fmtOData(record.get('InterfaceName'))
		};
		if (parameters) {
			for (i in parameters) {
				breezeParams[parameters[i].name] = parameters[i].value;
			}
		}
		breeze.EntityQuery
			.from(resource)
			.withParameters(breezeParams)
			.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
			.execute()
			.then(function (data) {
				grid.setLoading(false);
				var result = Ext.JSON.decode(data.httpResponse.data.value);
				if (result.success) {
					if (win) {
						win.close();
					}
					App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('ParseTaskMessage'));
					// Открыть панель задач
					controller.openUserTasksPanel();
				} else {
					App.Notify.pushError(result.message);
				}
			})
			.fail(function (data) {
				grid.setLoading(false);
				App.Notify.pushError(controller.getErrorMessage(data));
			});
	},

	onProcessApplyButtonClick: function (button) {
		var win = button.up('window');
		var form = win.down('form');
		if (form.isValid()) {
			var fields = Ext.ComponentQuery.query('field', form);
			var parameters = [];
			for (var i in fields) {
				var val = fields[i].getValue();
				var param = {
					name: fields[i].getName(),
					value: val ? val.toString() : val
				};
				parameters.push(param);
			}
			this.executeManualProcess(this, win.grid, win.resource, win.action, win.record, parameters, win);
		}
	},

	onInterfaceManualProcessButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button),
			selModel = grid.getSelectionModel();
		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			if (record.get('InterfaceDirection') !== 'INBOUND') {
				App.Notify.pushInfo('The interface is not inbound');
			} else {
				var resource = button.resource || 'FileBuffers';
				var action = button.action || 'ManualProcess';
				var paramsFormAlias = this.getManualProcessParameterForm(record.get('InterfaceName'));
				if (paramsFormAlias) {

					var editor = Ext.create('App.view.core.common.EditorWindow', {
						title: l10n.ns('core').value('processParamsWindowTitle'),
						grid: grid,
						resource: resource,
						action: action,
						record: record,
						name: 'processparamswindow',
						//width: 600,
						items: [
							Ext.widget(paramsFormAlias, {
								margin: '10 15 15 15'
							})
						],
						buttons: [{
							text: l10n.ns('core', 'buttons').value('cancel'),
							itemId: 'close'
						}, {
							text: l10n.ns('core', 'buttons').value('ok'),
							ui: 'green-button-footer-toolbar',
							itemId: 'apply'
						}]
					});
					editor.show();

				} else {
					me.executeManualProcess(me, grid, resource, action, record, null, null);
				}
			}
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionMessage'));
		}
	},

	onRestartButtonClick: function (button) {
		this.startJobInternal(button, 'LoopHandlers', 'Restart');
	},


	onStartJobButtonClick: function (button) {
		this.startJobInternal(button, 'LoopHandlers', 'Start');
	},

	startJobInternal: function (button, defaultResource, defaultAction) {
		var me = this;
		var grid = this.getGridByButton(button),
			selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			var resource = button.resource || 'LoopHandlers';
			var action = button.action || 'Start';

			grid.setLoading(true);
			breeze.EntityQuery
				.from(resource)
				.withParameters({
					$actionName: action,
					$method: 'POST',
					$entity: record.getProxy().getBreezeEntityByRecord(record)
				})
				.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
				.execute()
				.then(function (data) {
					grid.setLoading(false);
					var result = Ext.JSON.decode(data.httpResponse.data.value);
					if (result.success) {
						App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('CreateMessage'));
					} else {
						App.Notify.pushError(result.message);
					}
				})
				.fail(function (data) {
					grid.setLoading(false);
					App.Notify.pushError(me.getErrorMessage(data));
				});
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionTaskMessage'));
		}
	},

	getGridByButton: function (button) {
		return button.up('combineddirectorypanel').down('directorygrid');
	},

	getErrorMessage: function (data) {
		var result = 'Unknown error';
		if (data && data.body) {
			if (data.body["odata.error"]) {
				result = data.body["odata.error"].innererror.message;
			} else if (data.body.value) {
				result = data.body.value;
			}
		} else if (data && data.msg) {
			result = data.msg;
		} else if (data && data.message) {
			result = data.message;
		}
		return result;
	},

	onReadFileBufferLogButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button),
			selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			var resource = button.resource || 'FileBuffers';
			var action = button.action || 'ReadLogFile';

			grid.setLoading(true);
			breeze.EntityQuery
				.from(resource)
				.withParameters({
					$actionName: action,
					$method: 'POST',
					$entity: record.getProxy().getBreezeEntityByRecord(record),
					Id: breeze.DataType.Guid.fmtOData(record.get('Id')),
					CreateDate: breeze.DataType.DateTimeOffset.fmtOData(record.get('CreateDate'))
				})
				.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
				.execute()
				.then(function (data) {
					grid.setLoading(false);
					var resultData = data.httpResponse.data.value;
					me.openTextWindow(resultData, 'loophandlerviewlogwindow');
				})
				.fail(function (data) {
					grid.setLoading(false);
					App.Notify.pushError(me.getErrorMessage(data));
				});
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionMessage'));
		}
	},

	onReadLogButtonClick: function (button) {
		var me = this,
			window = button.up('window'),
			record = button.up('#taskform').getRecord();

		if (record) {
			var resource = button.resource || 'LoopHandlers';
			var action = button.action || 'ReadLogFile';

			window.setLoading(true);
			breeze.EntityQuery
				.from(resource)
				.withParameters({
					$actionName: action,
					$method: 'POST',
					$entity: record.getProxy().getBreezeEntityByRecord(record)
				})
				.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
				.execute()
				.then(function (data) {
					window.setLoading(false);
					var resultData = data.httpResponse.data.value;
					me.openTextWindow(resultData, 'loophandlerviewlogwindow');
				})
				.fail(function (data) {
					window.setLoading(false);
					App.Notify.pushError(me.getErrorMessage(data));
				});
		}
	},

	onTaskDetailsButtonClick: function (button) {
		var me = this,
			grid = me.getGridByButton(button),
			selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var window = Ext.widget('loophandlerdatawindow'),
				record = selModel.getSelection()[0],
				resource = button.resource || 'LoopHandlers',
				action = button.action || 'Parameters';

			window.down('#taskform').loadRecord(record);
			window.show();
			window.setLoading(true);

			breeze.EntityQuery
				.from(resource)
				.withParameters({
					$actionName: action,
					$method: 'POST',
					$entity: record.getProxy().getBreezeEntityByRecord(record)
				})
				.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
				.execute()
				.then(function (data) {
					var resultData = data.httpResponse.data.value;
					var result = JSON.parse(resultData);
					
					me.fillTaskResultForm(window.down('#resultform'), result['OutcomingParameters'], result, record);
					me.fillTaskDetailsForm(window.down('#parametersform'), result['IncomingParameters'], result, record);
					window.setLoading(false);
				})
				.fail(function (data) {
					window.setLoading(false);
					App.Notify.pushError(me.getErrorMessage(data));
				});
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionTaskMessage'));
		}
	},

	fillTaskResultForm: function (form, parameters, handlerData, record) {
		var models = this.buildParametersFieldList(form, parameters, handlerData, record)
		me = this;

		if (record.get('Status') === 'COMPLETE' || record.get('Status') === 'ERROR' || record.get('Status') === 'WARNING') {
			models.splice(0, 0, {
				xtype: 'fieldcontainer',
				cls: 'labelable-button',
				fieldLabel: l10n.ns('core', 'LoopHandler').value('DownloadLogTitle'),
				items: [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						// Открыть окно просмотра результата
						me.openDowloadLogFilesWindow(record.get('Id'));
					}
				}]
			});
			form.up('simplecombineddirectorypanel').minHeight = 50;

			Ext.ComponentQuery.query('#loophandlerlog')[0].enable();
		}

		if (models && models.length) {
			Ext.suspendLayouts();
			form.removeAll();
			form.add(models);
			Ext.resumeLayouts(true);
		} else {
			//Скрываем грид если нет данных для отображения
			form.up('simplecombineddirectorypanel').hide();
		}
	},

	fillTaskDetailsForm: function (form, parameters, handlerData, record) {
		var models = this.buildParametersFieldList(form, parameters, handlerData, record);

		if (models && models.length) {
			Ext.suspendLayouts();
			form.removeAll();
			form.add(models);
			Ext.resumeLayouts(true);
		} else {
			//Скрываем грид если нет данных для отображения
			form.up('simplecombineddirectorypanel').hide();
		}
	},

	onWindowResize: function (window) {
		window.center();
	},

	getFileDownloadLink: function (fileModel) {
		var pattern;
		switch (fileModel.LogicType) {
			case 'Import':
				pattern = '/api/File/ImportDownload?filename={0}';
				break;
			case 'Export':
				pattern = '/api/File/ExportDownload?filename={0}';
				break;
			case 'Interface':
				pattern = '/api/File/InterfaceDownload?filename={0}';
				break;
			case 'ImportSuccessResult':
				pattern = '/api/File/ImportResultSuccessDownload?filename={0}';
				break;
			case 'ImportWarningResult':
				pattern = '/api/File/ImportResultWarningDownload?filename={0}';
				break;
			case 'ImportErrorResult':
				pattern = '/api/File/ImportResultErrorDownload?filename={0}';
				break;
			case 'DataLakeSyncSuccessResult':
				pattern = '/api/File/DataLakeSyncResultSuccessDownload?filename={0}';
				break;
			case 'DataLakeSyncWarningResult':
				pattern = '/api/File/DataLakeSyncResultWarningDownload?filename={0}';
				break;
			case 'DataLakeSyncErrorResult':
				pattern = '/api/File/DataLakeSyncResultErrorDownload?filename={0}';
				break;
			case 'AdditionalFilesDownload':
				pattern = 'api/File/AdditionalFilesDownload?filename={0}';
				break;
			case 'InfoHandlerLogFileDownload':
				pattern = 'api/File/DownloadHandlerLogFile?filename={0}&type=Info';
				break;
			case 'WarningHandlerLogFileDownload':
				pattern = 'api/File/DownloadHandlerLogFile?filename={0}&type=Warning';
				break;
			case 'ErrorHandlerLogFileDownload':
				pattern = 'api/File/DownloadHandlerLogFile?filename={0}&type=Error';
				break;
			default:
				throw Ext.String.format("Logical file type '{0}' is not supported", fileModel.LogicType);
		}
		var url = document.location.href + Ext.String.format(pattern, fileModel.Name);
		var result = Ext.String.format('<a href="{0}" download="{1}" target="_blank">{1}</a>', url, fileModel.DisplayName);
		return result;
	},

	/**
	 * Открыть грид импорта с загуженными данными
	 * @param {string} widgetalias Name of window that must be shown
	 * @param {string} importId Id of import record
	 * @param {object} handlerData Parameters Data from server
	 * @param {object} parentGrid Grid that must be reloaded after import apply
	 */
	openActualImportWindow: function (widgetalias, importId, handlerData, parentWindow) {
		var widget = Ext.widget('basereviewwindow', {
			title: l10n.ns('core').value('actualImportWindowTitle'),
			itemId: 'viewactualimportwindow',
			items: [{
				xtype: widgetalias,
				parentWindow: parentWindow
			}]
		});
		var crossParams = {};
		if (handlerData && handlerData.IncomingParameters) {
			Ext.Object.each(handlerData.IncomingParameters, function (key, value) {
				if (Ext.String.startsWith(key, 'CrossParam.')) {
					crossParams[key] = value.Value;
				}
			}, this);
		}
		var grid = widget.down('directorygrid');
		grid.importData = {
			importId: importId,
			crossParams: crossParams
		};
		var store = grid.getStore();
		store.setFixedFilter('HistoricalObjectId', {
			property: 'ImportId',
			operation: 'Equals',
			value: importId
		});
		widget.show();
	},

	openViewFileListWindow: function (filter) {
		var widget = Ext.widget('basereviewwindow', {
			title: l10n.ns('core', 'compositePanelTitles').value('FileBufferTitle'),
			itemId: 'viewactualimportwindow',
			items: [{
				xtype: 'filebuffer'
			}]
		});
		var grid = widget.down('directorygrid');
		var store = grid.getStore();
		store.clearFixedFilters(true);
		Ext.Object.each(filter, function (name, value) {
			store.setFixedFilter(name, value);
		});
		widget.show();
	},

	onExtractApplyButtonClick: function (button) {
		var me = this;
		var win = button.up('window');
		var paramform = win.down('importparamform');
		var url = Ext.String.format("odata/{0}/{1}", win.resource, win.action);
		var isEmpty;

		if (paramform) {
			var constrains = paramform.query('field[isConstrain=true]');
			isEmpty = constrains && constrains.length > 0 && constrains.every(function (item) {
				return Ext.isEmpty(item.getValue());
			});

			if (isEmpty) {
				paramform.addCls('error-import-form');
				paramform.down('#errormsg').getEl().setVisible();
			}
		}

		if (paramform.isValid() && !isEmpty) {
			paramform.getForm().submit({
				url: url,
				success: function (fp, o) {
					// Проверить ответ от сервера на наличие ошибки и отобразить ее, в случае необходимости
					if (o.result.success) {
						App.Notify.pushInfo(win.successMessage);
						// Открыть панель задач
						me.openUserTasksPanel();
						if (win.needCloseParentAfterUpload && win.parentGrid) {
							win.parentGrid.up('window').close();
						}
						win.close();
					} else {
						App.Notify.pushError(o.result.message);
					}
				},
				failure: function (fp, o) {
					App.Notify.pushError(o.result.message);
				}
			});
		}
	},

	buildWidgetName: function (name, prefix, suffix) {
		return Ext.String.format('{0}{1}{2}', prefix, name, suffix).toLowerCase();
	},

	openManualExtractWindow: function (form, handlerData, resource, action) {
		var paramsFormName = handlerData.IncomingParameters['ManualExtract'].Value.ParamsForm;
		var viewClassName = this.buildWidgetName(paramsFormName, 'Extract', 'ParamForm');

		var editor = Ext.create('App.view.core.common.EditorWindow', {
			title: l10n.ns('core').value('extractParamsWindowTitle'),
			parentGrid: form,
			needCloseParentAfterUpload: true,
			successMessage: l10n.ns('core', 'LoopHandler').value('ManualDataExtractionTask'),
			resource: resource,
			action: action,
			name: 'extractparamswindow',
			width: 600,
			items: [
				Ext.widget(viewClassName, {
					margin: '10 15 15 15'
				})
			],
			buttons: [{
				text: l10n.ns('core', 'buttons').value('cancel'),
				itemId: 'close'
			}, {
				text: l10n.ns('core', 'buttons').value('ok'),
				ui: 'green-button-footer-toolbar',
				itemId: 'apply'
			}]
		});
		editor.down('editorform').add([{
			xtype: 'hiddenfield',
			name: 'InterfaceId',
			value: handlerData.IncomingParameters['InterfaceId'].Value
		}, {
			xtype: 'hiddenfield',
			name: 'InterfaceName',
			value: handlerData.IncomingParameters['InterfaceName'].Value
		}]);
		editor.show();
	},

	openImportResultFilesWindow: function (taskId) {
		var win = Ext.widget('importresultfileswindow');
		function buildField(type) {
			return {
				xtype: 'singlelinedisplayfield',
				name: type + 'File',
				fieldLabel: l10n.ns('core', 'FileBuffer', 'ImportResultFileTypes').value(type),
				value: this.getFileDownloadLink({ LogicType: 'Import' + type + 'Result', DisplayName: l10n.ns('core', 'buttons').value('download'), Name: taskId })
			};
		};
		var fields = [buildField.call(this, 'Success'), buildField.call(this, 'Warning'), buildField.call(this, 'Error')];
		win.down('#importresultfilesform').add(fields);
		win.show();
	},

	openDowloadLogFilesWindow: function (taskId) {
		var win = Ext.widget('importresultfileswindow');
		win.title = l10n.ns('core', 'LoopHandler').value('DownloadLogTitle');
		function buildField(type) {
			return {
				xtype: 'singlelinedisplayfield',
				name: type + 'File',
				fieldLabel: l10n.ns('core', 'HandlerLogFileDownload').value(type),
				value: this.getFileDownloadLink(
					{
						LogicType: type + 'HandlerLogFileDownload',
						DisplayName: l10n.ns('core', 'buttons').value('download'),
						Name: taskId + '.txt'
					})
			};
		};
		var fields = [buildField.call(this, 'Info'), buildField.call(this, 'Warning'), buildField.call(this, 'Error')];
		win.down('#importresultfilesform').add(fields);
		win.show();
	},

	openDataLakeSyncResultFilesWindow: function (taskId) {
		var win = Ext.widget('datalakesyncresultfileswindow');
		function buildField(type) {
			return {
				xtype: 'singlelinedisplayfield',
				name: type + 'File',
				fieldLabel: l10n.ns('core', 'FileBuffer', 'DataLakeSyncResultFileTypes').value(type),
				value: this.getFileDownloadLink({ LogicType: 'DataLakeSync' + type + 'Result', DisplayName: l10n.ns('core', 'buttons').value('download'), Name: taskId })
			};
		};
		var fields = [buildField.call(this, 'Success'), buildField.call(this, 'Warning'), buildField.call(this, 'Error')];
		win.down('#datalakesyncresultfilesform').add(fields);
		win.show();
	},

	onImportResultFilesButtonClick: function (button) {
		var me = this;
		var grid = this.getGridByButton(button),
			selModel = grid.getSelectionModel();

		if (selModel.hasSelection()) {
			var record = selModel.getSelection()[0];
			if (record.get('InterfaceDirection') !== 'INBOUND') {
				App.Notify.pushInfo('The interface is not inbound');
			} else {
				this.openImportResultFilesWindow(record.get('Id'));
			}
		} else {
			console.log('No selection');
			App.Notify.pushInfo(l10n.ns('core', 'LoopHandler').value('NoSelectionMessage'));
		}
	},

	openInterfaceCollectWindow: function (form, handlerData, resource, action) {
		var editor = Ext.create('App.view.core.common.UploadFileWindow', {
			title: l10n.ns('core').value('uploadFileWindowTitle'),
			parentGrid: form,
			needCloseParentAfterUpload: true,
			successMessage: 'The task of manually collecting the interface file has been successfully created',
			resource: resource,
			action: action
		});
		editor.down('editorform').add([{
			xtype: 'hiddenfield',
			name: 'InterfaceId',
			value: handlerData.IncomingParameters['InterfaceId'].Value
		}, {
			xtype: 'hiddenfield',
			name: 'InterfaceName',
			value: handlerData.IncomingParameters['InterfaceName'].Value
		}]);
		editor.show();
	},

	getViewFileListFilter: function (param, handlerData, record) {
		var filter = {};
		if (param.Value.FilterType === 'INTERFACE') {
			filter['InterfaceFilter'] = {
				property: 'InterfaceId',
				operation: 'Equals',
				value: handlerData.IncomingParameters['InterfaceId'].Value
			};
		} else if (param.Value.FilterType === 'HANDLER') {
			filter['HandlerFilter'] = {
				property: 'HandlerId',
				operation: 'Equals',
				value: record.get('Id')
			};
		} else if (param.Value.FilterType === 'FILE') {
			var fileBuffer = handlerData.IncomingParameters['FileBuffer'].Value;
			filter['IdFilter'] = {
				property: 'Id',
				operation: 'Equals',
				value: fileBuffer.FileBufferId
			};
			filter['CreateDateFilter'] = {
				property: 'CreateDate',
				operation: 'Equals',
				value: fileBuffer.FileBufferCreateDate
			};
		} else {
			throw 'FilterType "' + param.Value.FilterType + '" is not supported';
		}
		return filter;
	},

	openUserTasksPanel: function () {
		var systemPanel = Ext.ComponentQuery.query('system')[0];
		var tasksTab = Ext.ComponentQuery.query('#systemUserTasksTab')[0];
		if (systemPanel && tasksTab) {
			systemPanel.setActiveTab(tasksTab);
		}
	},

	buildParameterField: function (param, handlerData, record, form) {
		var me = this;
		var type = this.getShortTypeName(param.ValueType);
		var model = {
			xtype: 'singlelinedisplayfield',
			name: param.Name,
			fieldLabel: l10n.ns('core', 'LoopHandler', 'ParameterNames').value(param.Name) || 'Unknown Parameter',
			value: param.Value
		};
		switch (type) {
			case 'System.String':
				model.xtype = 'singlelinedisplayfield';
				model.value = param.Value;
				break;
			case 'System.DateTimeOffset':
			case 'System.DateTime':
				model.xtype = 'singlelinedisplayfield';
				model.value = Ext.Date.format(new Date(param.Value), 'Y.m.d H:i');
				break;
			case 'System.Boolean':
				model.xtype = 'singlelinedisplayfield';
				var isNull = param.Value === null || param.Value === undefined;
				model.value = isNull ? '' : param.Value.toString();
				break;
			case 'Looper.Parameters.FileModel':
				model.xtype = 'singlelinedisplayfield';
				if (record.data.Status == 'COMPLETE' || record.data.Name != 'Module.Host.TPM.Handlers.SchedulerExportHandler') {
					model.value = this.getFileDownloadLink(param.Value);
				}
				else
					model.value = '';
				break;
			case 'Looper.Parameters.ImportResultModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						console.log(param.Value.ImportModelType);
						console.log(param.Value.ImportId);
						var widgetalias = param.Value.ImportModelType.toLowerCase();
						me.openActualImportWindow(widgetalias, param.Value.ImportId, handlerData, button.up('window'));
					}
				}];
				break;
			case 'Looper.Parameters.InterfaceFileListModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						var filter = me.getViewFileListFilter(param, handlerData, record);
						me.openViewFileListWindow(filter);
					}
				}];
				break;
			case 'Looper.Parameters.IntegrityCheckResultModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						// Открыть окно просмотра результата
						var text = param.Value.HeaderText + param.Value.Data;
						me.openTextWindow(text, 'loophandlerviewlogwindow');
					}
				}];
				break;
			case 'Looper.Parameters.ManualFileCollectModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						var resource = param.Value.Resource || 'Interfaces';
						var action = param.Value.Action || 'ManualCollect';
						me.openInterfaceCollectWindow(form, handlerData, resource, action);
					}
				}];
				break;
			case 'Looper.Parameters.ManualExtractModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						var resource = param.Value.Resource || 'Interfaces';
						var action = param.Value.Action || 'ManualExtract';
						me.openManualExtractWindow(form, handlerData, resource, action);
					}
				}];
				break;
			case 'Looper.Parameters.ImportResultFilesModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						me.openImportResultFilesWindow(param.Value.TaskId);
					}
				}];
				break;
			case 'Looper.Parameters.DataLakeSyncResultFilesModel':
				model.xtype = 'fieldcontainer';
				model.cls = 'labelable-button';
				model.items = [{
					xtype: 'button',
					text: l10n.ns('core', 'buttons').value('open'),
					handler: function (button) {
						me.openDataLakeSyncResultFilesWindow(param.Value.TaskId);
					}
				}];
				break;
			case 'Looper.Parameters.TextListModel':
				model.xtype = 'listsinglelinetriggerfield';
				model.separator = param.Value ? param.Value.Separator : ';';
				model.value = param.Value ? param.Value.Value : '';
				break;
			//case 'Looper.Parameters.ManualFileProcessModel':
			//    model.xtype = 'fieldcontainer';
			//    model.cls = 'labelable-button';
			//    model.items = [{
			//        xtype: 'button',
			//        text: l10n.ns('core', 'buttons').value('open'),
			//        handler: function (button) {
			//            var resource = param.Value.Resource || 'Interfaces';
			//            var action = param.Value.Action || 'ManualProcess';
			//            me.manualInterfaceProcessButtonClick(form)
			//            //// TODO!!!
			//            //var editor = Ext.create('App.view.core.common.UploadFileWindow', {
			//            //    title: l10n.ns('core').value('uploadFileWindowTitle'),
			//            //    parentGrid: form,
			//            //    needCloseParentAfterUpload: true,
			//            //    successMessage: 'Задача ручного сбора интерфейсного файла успешно создана',
			//            //    resource: resource,
			//            //    action: action
			//            //});
			//            //debugger;
			//            //editor.down('editorform').add([{
			//            //    xtype: 'hiddenfield',
			//            //    name: 'InterfaceId',
			//            //    value: handlerData.IncomingParameters['InterfaceId'].Value
			//            //}, {
			//            //    xtype: 'hiddenfield',
			//            //    name: 'InterfaceName',
			//            //    value: handlerData.IncomingParameters['InterfaceName'].Value
			//            //}]);
			//            //editor.show();
			//        }
			//    }];
			//    break;
			case 'Module.Persist.QDMS.Model.QDMS.MatmasMaterial':
				model.xtype = 'singlelinedisplayfield';
				model.value = param.Value.length;
				break;
			case 'Module.Persist.QDMS.Model.QDMS.InfinitySetting':
				model.xtype = 'singlelinedisplayfield';
				model.value = param.Value.length;
				break;
			default:
				break;
		}
		return model;
	},

	openTextWindow: function (text, wigdetName) {
		var win = Ext.widget(wigdetName);//Ext.create('App.view.core.loophandler.ViewLogWindow');
		var textField = win.down('field[name=logtext]');
		if (textField) {
			textField.setValue(text);
			win.show();
		} else {
			console.log('field[name=logtext] Not Found!!!');
			App.Notify.pushError(l10n.ns('core', 'LoopHandler').value('LogWindowError'));
		}
	},

	getShortTypeName: function (type) {
		var commaResult = type.split(',')[0];
		var backetResult = commaResult.split('[');
		return backetResult[backetResult.length - 1];
	},

	/** 
	 * Преобразовать объект HandlerData в список моделей, пригодных для отображения
	 * @param {object} parametersObject Parameters Data from server
	 * @param {object} handlerData Parameters Data from server
	 * @returns {array} List of model for displey on the form
	 */
	buildParametersFieldList: function (form, parametersObject, handlerData, record) {
		var result = [];
		if (parametersObject) {
			Ext.Object.each(parametersObject, function (key, value) {
				if (value.Visible) {
					var model = this.buildParameterField(value, handlerData, record, form);
					result.push(model);
				}
			}, this);
		}
		return result;
	},

	onStartButtonClick: function (button) {
		Ext.Msg.alert('Button click', 'Start button click. Not implemented');
	},


	onStopButtonClick: function (button) {
		Ext.Msg.alert('Button click', 'Stop button click. Not implemented');
	}

});