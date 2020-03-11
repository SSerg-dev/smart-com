Ext.define('App.controller.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalfilecollectinterfacesetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalfilecollectinterfacesetting directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalfilecollectinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalfilecollectinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalfilecollectinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalfilecollectinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalfilecollectinterfacesetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalfilecollectinterfacesetting #next': {
                    click: this.onNextButtonClick
                },
                'historicalfilecollectinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalfilecollectinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalfilecollectinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
