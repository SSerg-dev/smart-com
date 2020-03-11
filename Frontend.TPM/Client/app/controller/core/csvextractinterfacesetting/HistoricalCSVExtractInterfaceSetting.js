Ext.define('App.controller.core.csvextractinterfacesetting.HistoricalCSVExtractInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcsvextractinterfacesetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcsvextractinterfacesetting directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcsvextractinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcsvextractinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcsvextractinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcsvextractinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalcsvextractinterfacesetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcsvextractinterfacesetting #next': {
                    click: this.onNextButtonClick
                },
                'historicalcsvextractinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcsvextractinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcsvextractinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
