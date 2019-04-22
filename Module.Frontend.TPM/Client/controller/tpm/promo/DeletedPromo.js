Ext.define('App.controller.tpm.promo.DeletedPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromo directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromo #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromo #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromo #detail': {
                    click: this.switchToDetailForm
                },
                'deletedpromo #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromo #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },

    switchToDetailForm: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
            grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
                grid: grid
            });
        }    
        if (selModel.hasSelection()) {
            grid.editorModel.startDetailRecord(selModel.getSelection()[0]);
        } else {
            console.log('No selection');
        }
    }
});
