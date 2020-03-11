Ext.define('App.controller.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalfilesendinterfacesetting directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalfilesendinterfacesetting directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalfilesendinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'historicalfilesendinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'historicalfilesendinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalfilesendinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'historicalfilesendinterfacesetting #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalfilesendinterfacesetting #next': {
                    click: this.onNextButtonClick
                },
                'historicalfilesendinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalfilesendinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalfilesendinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
