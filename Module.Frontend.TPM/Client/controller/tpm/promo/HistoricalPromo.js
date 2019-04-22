Ext.define('App.controller.tpm.promo.HistoricalPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromo directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange,
                },
                'historicalpromo #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromo #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromo #detail': {
                    click: this.switchToDetailForm
                },
                'historicalpromo #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromo #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromo #close': {
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
