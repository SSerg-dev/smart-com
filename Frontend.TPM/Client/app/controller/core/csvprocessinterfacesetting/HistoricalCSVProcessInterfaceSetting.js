Ext.define('App.controller.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcsvprocessinterfacesetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcsvprocessinterfacesetting directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcsvprocessinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcsvprocessinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcsvprocessinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcsvprocessinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalcsvprocessinterfacesetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcsvprocessinterfacesetting #next': {
                    click: this.onNextButtonClick
                },
                'historicalcsvprocessinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcsvprocessinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcsvprocessinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
