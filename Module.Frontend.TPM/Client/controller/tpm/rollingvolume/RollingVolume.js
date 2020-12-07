Ext.define('App.controller.tpm.rollingvolume.RollingVolume', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rollingvolume[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'rollingvolume directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rollingvolume #datatable': {
                    activate: this.onActivateCard
                },
                'rollingvolume #detailform': {
                    activate: this.onActivateCard
                },
                'rollingvolume #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rollingvolume #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rollingvolume #detail': {
                    click: this.onDetailButtonClick
                },
                'rollingvolume #table': {
                    click: this.onTableButtonClick
                },
                'rollingvolume #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rollingvolume #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'rollingvolume #createbutton': {
                    click: this.onCreateButtonClick
                },
                'rollingvolume #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'rollingvolume #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'rollingvolume #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'rollingvolume #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rollingvolume #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'rollingvolume #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rollingvolume #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rollingvolume #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'rollingvolume #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onGridSelectionChange: function (grid) {
        var recalculatePreviousYearButton = grid.view.up('panel').up().down('#updatebutton');
        var disabled = recalculatePreviousYearButton.disabled;
        this.callParent(arguments);
        recalculatePreviousYearButton.setDisabled(disabled);
    },

    onDetailButtonClick: function (button) {
        this.callParent(arguments);
        var rollingvolumeeditor = Ext.ComponentQuery.query('rollingvolumeeditor')[0];     
        var rollingEditButton = button.up('panel').up().down('#updatebutton');
        var disabled = rollingEditButton.disabled;
        rollingvolumeeditor.down('#edit').setVisible(!disabled);
    },

    
    onGridAfterrender: function (grid) {
        this.callParent(arguments);
    
        var resource = 'RollingVolumes',
            action = 'IsRollingVolumeDay',
            allowedActions = [];

        var currentRoleAPs = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
            return point.Resource === resource;
        });

        currentRoleAPs.forEach(function (point) {
            if (!allowedActions.some(function (action) { return action === point.Action })) {
                allowedActions.push(point.Action);
            }
        });
        if (Ext.Array.contains(allowedActions, action)) {
            var store = grid.getStore();
            store.on('load', function (store) {
                parameters = {};

                App.Util.makeRequestWithCallback(resource, action, parameters, function (data) {
                    if (data) {
                        var result = Ext.JSON.decode(data.httpResponse.data.value);
                        if (result.success) {
                            var recalculatePreviousYearButton = grid.up('panel')
                                
                            if (!result.isDayOfWeek) {
                                recalculatePreviousYearButton.down('#updatebutton').setDisabled(true);
                                recalculatePreviousYearButton.down('#ImportXLSX').visible = false;
                                recalculatePreviousYearButton.down('#ImportXLSX').setVisible(false);
                            } else {
                                recalculatePreviousYearButton.down('#updatebutton').setDisabled(false);
                                recalculatePreviousYearButton.down('#ImportXLSX').visible = true;
                                recalculatePreviousYearButton.down('#ImportXLSX').setVisible(true);
                            }

                        } else {
                            App.Notify.pushError(data.message);
                        }
                    }
                }, function (data) {
                    App.Notify.pushError(data.message);
                });
            });
        }
    },
});
