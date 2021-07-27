Ext.define('App.controller.tpm.pludictionary.HistoricalPLUDictionary', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpludictionary directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpludictionary directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpludictionary #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpludictionary #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpludictionary #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpludictionary #table': {
                    click: this.onTableButtonClick
                },
                'historicalpludictionary #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpludictionary #next': {
                    click: this.onNextButtonClick
                },
                'historicalpludictionary #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpludictionary #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpludictionary #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
