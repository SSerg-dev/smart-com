Ext.define('App.controller.tpm.rsmode.RSmode', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rsmode[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'rsmode directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onRsGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rsmode #datatable': {
                    activate: this.onActivateCard
                },
                'rsmode #detailform': {
                    activate: this.onActivateCard
                },
                'rsmode #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rsmode #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rsmode #detail': {
                    click: this.switchToDetailForm
                },
                'rsmode #table': {
                    click: this.onTableButtonClick
                },
                'rsmode #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rsmode #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'rsmode #onapprovalbutton': {
                    click: this.onOnApprovalButtonClick
                },
                'rsmode #approvebutton': {
                    click: this.onApproveButtonClick
                },
                'rsmode #massapprovebutton': {
                    click: this.onMassApproveButtonClick
                },
                'rsmode #declinebutton': {
                    click: this.onDeclineButtonClick
                },
                'rsmode #calculatebutton': {
                    click: this.onCalculateButtonClick
                },
                'rsmode #showlogbutton': {
                    click: this.onShowLogButtonClick
                },
                'rsmode #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'rsmode #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'rsmode #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'rsmode #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rsmode #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'rsmode #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rsmode #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rsmode #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'rsmode #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
    getRSPeriod: function (cb_func) {
        var tpmMode = TpmModes.getSelectedModeId();
        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/GetRSPeriod?TPMmode=' + tpmMode,
            type: 'POST',
            async: false,
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    cb_func(data.startEndModel);
                }
                else {
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                }
            },
            error: function (data) {
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },
    onDetailButtonClick: function () {

    },
    onRsGridAfterrender: function (grid) {

        this.onGridAfterrender(grid);
    },
    onOnApprovalButtonClick: function (button) {
        var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
        grid.setLoading(l10n.ns('core').value('savingText'));
        var selected = grid.getSelectionModel().getSelection()[0];
        $.ajax({
            dataType: 'json',
            url: '/odata/RollingScenarios/OnApproval?rollingScenarioId=' + selected.data.Id,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    grid.getStore().load();
                    grid.setLoading(false);
                }
                else {
                    grid.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                grid.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },
    onApproveButtonClick: function (button) {
        Ext.Msg.show({
            title: l10n.ns('tpm', 'text').value('Confirmation'),
            msg: 'Warning! After scenario approving, an irreversible synchronization with the production calendar will occur.',
            fn: function (btn) {
                if (btn === 'yes') {
                    var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
                    grid.setLoading(l10n.ns('core').value('savingText'));
                    var selected = grid.getSelectionModel().getSelection()[0];
                    $.ajax({
                        dataType: 'json',
                        url: '/odata/RollingScenarios/Approve?rollingScenarioId=' + selected.data.Id,
                        type: 'POST',
                        success: function (response) {
                            var data = Ext.JSON.decode(response.value);
                            if (data.success) {
                                grid.getStore().load();
                                grid.setLoading(false);
                            }
                            else {
                                grid.setLoading(false);
                                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                            }

                        },
                        error: function (data) {
                            grid.setLoading(false);
                            App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
                        }
                    });
                }
            },
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO,
            buttonText: {
                yes: l10n.ns('tpm', 'button').value('confirm'),
                no: l10n.ns('tpm', 'button').value('cancel')
            }
        });

    },
    onMassApproveButtonClick: function (button) {
        debugger;
    },
    onShowLogButtonClick: function (button) {
        var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
        var selected = grid.getSelectionModel().getSelection()[0];

        if (!Ext.isEmpty(selected.data.HandlerId)) {
            var calculatingInfoWindow = Ext.create('App.view.tpm.promocalculating.CalculatingInfoWindow');
            calculatingInfoWindow.on({
                beforeclose: function() {
                    if ($.connection.tasksLogHub)
                        requestHub($.connection.tasksLogHub.server.unsubscribeLog, [selected.data.HandlerId]);
                }
            });
            
            calculatingInfoWindow.show();
            requestHub($.connection.tasksLogHub.server.subscribeLog, [selected.data.HandlerId]);
        }
    },
    onCalculateButtonClick: function(button) {
        var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
        grid.setLoading(l10n.ns('core').value('savingText'));
        var selected = grid.getSelectionModel().getSelection()[0];
        $.ajax({
            dataType: 'json',
            url: '/odata/RollingScenarios/Calculate?rsId=' + selected.data.RSId,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    grid.getStore().load();
                    grid.setLoading(false);
                    App.Notify.pushInfo('Calculating task created successfully');
                    App.System.openUserTasksPanel();
                }
                else {
                    grid.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                grid.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });

    },
    onDeclineButtonClick: function (button) {
        var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
        grid.setLoading(l10n.ns('core').value('savingText'));
        var selected = grid.getSelectionModel().getSelection()[0];
        $.ajax({
            dataType: 'json',
            url: '/odata/RollingScenarios/Decline?rollingScenarioId=' + selected.data.Id,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    grid.getStore().load();
                    grid.setLoading(false);
                }
                else {
                    grid.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                grid.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },
    getVisibleButton: function (rollingScenarioId) {
        $.ajax({
            dataType: 'json',
            url: '/odata/RollingScenarios/GetVisibleButton?rollingScenarioId=' + rollingScenarioId,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    Ext.ComponentQuery.query('button[itemId=onapprovalbutton]')[0].setDisabled(data.OnApproval);
                    Ext.ComponentQuery.query('button[itemId=approvebutton]')[0].setDisabled(data.Approve);
                    Ext.ComponentQuery.query('button[itemId=declinebutton]')[0].setDisabled(data.Decline);
                    Ext.ComponentQuery.query('button[itemId=calculatebutton]')[0].setDisabled(data.Calculate);
                    Ext.ComponentQuery.query('button[itemId=showlogbutton]')[0].setDisabled(data.ShowLog);
                }
            },
            error: function (data) {
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },
    onGridSelectionChange: function (selModel, selected) {
        this.callParent(arguments);
        var me = this;
        var grid = selModel.view.up('directorygrid');
        if (selected.length > 0) {
            this.getVisibleButton(selected[0].data.Id);
        }
    },
});
