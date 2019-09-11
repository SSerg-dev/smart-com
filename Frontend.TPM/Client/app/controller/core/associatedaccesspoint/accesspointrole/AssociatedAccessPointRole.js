Ext.define('App.controller.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole', {
    extend: 'App.controller.core.AssociatedDirectory',
	mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'associatedaccesspointaccesspointrole directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'associatedaccesspointaccesspointrole #datatable': {
                    activate: this.onActivateCard
                },
                'associatedaccesspointaccesspointrole #detailform': {
                    activate: this.onActivateCard
                },                             
                'associatedaccesspointaccesspointrole #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associatedaccesspointaccesspointrole #detailform #next': {
                    click: this.onNextButtonClick
                },
                //

                'associatedaccesspointaccesspointrole #detail': {
                    click: this.switchToDetailForm
                },
                'associatedaccesspointaccesspointrole #table': {
                    click: this.onTableButtonClick
                },
                'associatedaccesspointaccesspointrole #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associatedaccesspointaccesspointrole #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associatedaccesspointaccesspointrole #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associatedaccesspointaccesspointrole #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associatedaccesspointaccesspointrole #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associatedaccesspointaccesspointrole #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associatedaccesspointaccesspointrole #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associatedaccesspointaccesspointrole #close': {
                    click: this.onCloseButtonClick
                },

	            // import/export
                'associatedaccesspointaccesspointrole #exportbutton': {
                    click: this.onExportButtonClick
                },
                'associatedaccesspointaccesspointrole #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'associatedaccesspointaccesspointrole #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'associatedaccesspointaccesspointrole #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'associatedaccesspointaccesspointrole #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'associatedaccesspointaccesspointrole #addbutton': {
                    click: this.onAddButtonClick
                },
                '#associatedaccesspointcontainer_role_selectorwindow directorygrid': {
                    selectionchange: this.onSelectorGridSelectionChange
                },
                '#associatedaccesspointcontainer_role_selectorwindow #select': {
                    click: this.onSelectButtonClick
                }
            }
        });
    },

    getSaveModelConfig: function (record, grid) {
        var ownerGrid = grid.up('window').ownerGrid,
            parentPanel = ownerGrid.up('combineddirectorypanel').getParent();

        if (parentPanel) {
            return {
                AccessPointId: parentPanel.down('directorygrid').getSelectionModel().getSelection()[0].getId(),
                RoleId: record.getId()
            };
        }
    },

    getSelectorPanel: function () {
        return Ext.widget('role');
    }

});