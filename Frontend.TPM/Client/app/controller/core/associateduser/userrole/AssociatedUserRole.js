Ext.define('App.controller.core.associateduser.userrole.AssociatedUserRole', {
    extend: 'App.controller.core.AssociatedDirectory',
	mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'associateduseruserrole directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'associateduseruserrole #datatable': {
                    activate: this.onActivateCard
                },
                'associateduseruserrole #detailform': {
                    activate: this.onActivateCard
                },                
                'associateduseruserrole #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associateduseruserrole #detailform #next': {
                    click: this.onNextButtonClick
                },
                //

                'associateduseruserrole #detail': {
                    click: this.switchToDetailForm
                },
                'associateduseruserrole #table': {
                    click: this.onTableButtonClick
                },
                'associateduseruserrole #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associateduseruserrole #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associateduseruserrole #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associateduseruserrole #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associateduseruserrole #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associateduseruserrole #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associateduseruserrole #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associateduseruserrole #close': {
                    click: this.onCloseButtonClick
                },

	            // import/export
                'associateduseruserrole #exportbutton': {
                    click: this.onExportButtonClick
                },
                'associateduseruserrole #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'associateduseruserrole #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'associateduseruserrole #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'associateduseruserrole #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'associateduseruserrole #addbutton': {
                    click: this.onAddButtonClick
                },
                '#associateddbusercontainer_role_selectorwindow directorygrid': {
                    selectionchange: this.onSelectorGridSelectionChange
                },
                '#associateddbusercontainer_role_selectorwindow #select': {
                    click: this.onSelectButtonClick
                },
                '#associatedadusercontainer_role_selectorwindow directorygrid': {
                    selectionchange: this.onSelectorGridSelectionChange
                },
                '#associatedadusercontainer_role_selectorwindow #select': {
                    click: this.onSelectButtonClick
                },

                'associateduseruserrole #setdefaultbutton': {
                    click: this.onSetDefaultButtonClick
                }
            }
        });
    },

    onGridSelectionChange: function (selModel) {
        this.callParent(arguments);

        selModel.view
            .up('combineddirectorypanel')
            .down('#setdefaultbutton')
            .setDisabled(!selModel.hasSelection());
    },

    getSaveModelConfig: function (record, grid) {
        var ownerGrid = grid.up('window').ownerGrid,
            parentPanel = ownerGrid.up('combineddirectorypanel').getParent();

        if (parentPanel) {
            return {
                UserId: parentPanel.down('directorygrid').getSelectionModel().getSelection()[0].getId(),
                RoleId: record.getId(),
                IsDefault: false
            };
        }
    },

    getSelectorPanel: function () {
        return Ext.widget('role');
    },

    onSetDefaultButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('confirmTitle'),
                msg: l10n.ns('core').value('defaultRoleConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('appoint'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {
            if (buttonId === 'yes') {
                var record = selModel.getSelection()[0],
                    panel = grid.up('combineddirectorypanel');

                panel.setLoading(true);
                breeze.EntityQuery
                    .from('UserRoles')
                    .withParameters({
                        $actionName: 'SetDefault',
                        $method: 'POST',
                        $entity: record.getProxy().getBreezeEntityByRecord(record)
                    })
                    .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                    .execute()
                    .then(function () {
                        grid.setLoadMaskDisabled(true);
                        grid.getStore().on({
                            single: true,
                            load: function (records, operation, success) {
                                panel.setLoading(false);
                                grid.setLoadMaskDisabled(false);
                            }
                        });
                        grid.getStore().load();

                        var activeView = panel.getLayout().getActiveItem();
                        if (activeView.getItemId() === 'detailform') {
                            var activeForm = activeView.down('form'),
                                formRecord = activeForm.getRecord();

                            if (formRecord && formRecord.getId() === record.getId()) {
                                formRecord.set('IsDefault', true);
                                activeForm.loadRecord(formRecord);
                            }
                        }
                    })
                    .fail(function () {
                        panel.setLoading(false);
                    });
            }
        }
    }

});