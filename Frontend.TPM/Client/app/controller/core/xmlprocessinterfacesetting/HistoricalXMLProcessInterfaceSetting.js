Ext.define('App.controller.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalxmlprocessinterfacesetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalxmlprocessinterfacesetting directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalxmlprocessinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalxmlprocessinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalxmlprocessinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalxmlprocessinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalxmlprocessinterfacesetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalxmlprocessinterfacesetting #next': {
                    click: this.onNextButtonClick
                },
                'historicalxmlprocessinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalxmlprocessinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalxmlprocessinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
