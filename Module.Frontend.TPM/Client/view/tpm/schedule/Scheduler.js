Ext.define('App.view.tpm.schedule.Scheduler', {
    extend: 'Sch.panel.SchedulerGrid',
    xtype: 'scheduler',
    itemId: 'nascheduler',
    header: false,
    isMarsMode: true,
    autoAdjustTimeAxis: false,
    scroll: 'vertical',
    overflowX: false,
    enableEventDragDrop: true,
    constrainDragToResource: true,
    enableDragCreation: true,
    controller: 'schedulerviewcontroller',
    viewPreset: 'marsweekMonth',
    loadMask: true,
    rowHeight: 36,
    
    //startDate: new Date(new Date().getFullYear() - 1, 10, 01),
    //endDate: new Date(new Date().getFullYear() + 1, 3, 01),
    startDate: new Date(new Date().getFullYear(), new Date().getMonth() - 1, 1),
    endDate: new Date(new Date().getFullYear(), new Date().getMonth() + 2, 0),
    // workaround для отключения прокрутки к началу строки по клику на неё  
    viewConfig: {
        focusRow: Ext.emptyFn,
        loadingText: 'Loading promoes',
    },
    listeners: {
        render: function (el) {
            var schedulergridview = el.down('schedulergridview');
            var grid = el.down('gridview');

            $('#' + schedulergridview.id + ', #' + grid.id).on('wheel', function (e) {
                var direction = e.originalEvent.deltaY > 0 ? 1 : -1;
                var scrollValue = this.scrollTop + direction * 40;

                $('#scrollScheduler').data('jsp').scrollToY(scrollValue);
                return false;
            });
        },
    },

    horizontalTimeAxisColumnCfg: {
        enableTickResizing: true
    },

    plugins: [
        //{
        //ptype: 'bufferedrenderer',
        //trailingBufferZone: 1,
        //leadingBufferZone: 1,
        //variableRowHeight: true
        //},
        {
            // Плагин для отрисовки линий на календаре
            ptype: 'scheduler_lines',
            showHeaderElements: true,
            store: {
                type: 'schedulerlinestore',
                data: [{
                    Date: new Date(),
                    Text: 'Today',
                    Cls: 'current-day-line'
                }]
            }
        }],

    createConfig: {
        // кастомный тултип календаря
        hoverTip: {
            getText: function (date, e) {
                if (date > new Date()) {
                    return l10n.ns('tpm', 'Scheduler').value('CanCreate');
                } else {
                    return l10n.ns('tpm', 'Scheduler').value('CannotCreate');
                }
            }
            // TODO: изменить clockTpl - метод initComponent()
            // возможно придётся переписать updateHoverTip()
        },
    },

    // настройки тултипа при наведении на промо
    tooltipTpl: new Ext.XTemplate('<dl class="eventTip">' +
        '<dt>Name</dt><dd>{Name}</dd>' +
        '<dt>Mechanic</dt><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;">{MarsMechanicName} <tpl if="MarsMechanicName === \'TPR\' || MarsMechanicName === \'Other\'">{MarsMechanicDiscount}%</tpl><tpl if="MarsMechanicName === \'VP\'">{MarsMechanicTypeName}</tpl></dd>' +
        '<dl style="border-left: 5px solid {PromoStatusColor}; margin: 0px;"><dt style="margin: 0 0 3px 2px;">Status</dt><dd style="margin: 0 0 3px 2px;">{PromoStatusSystemName}</dd></dl>' +
        '</dl>'),
    tipCfg: {
        cls: 'sch-tip',
        height: '120px',
        showDelay: 200,
        hideDelay: 200,
        autoHide: true,
        anchor: 'b',
        show: function () { // если высота меньше чем высота подсказки, подсказка начинает мерцать - баг в функции show плагина
            Ext.ToolTip.prototype.show.apply(this, arguments);
        }
    },
    // Промо в две строки
    eventBodyTemplate: '<div class="event-status-border" style ="border-left: 5px solid {statusColor};" ><span class="sch-event-text">{headerText}</span></div>',

    // возвращает объект заполняющий eventBodyTemplate
    eventRenderer: function (event, resource, tplData, row, col, ds) {
        var bgColor = event.get('ColorSystemName');
        tplData.style = Ext.String.format('background: {0} !important; border-color: white;', bgColor || '#DDDDDD');
        return {
            headerText: event.get('Name'),
            statusColor: event.get('PromoStatusColor') || '#DDDDDD'
        };
    },

    columns: [{
        header: l10n.ns('tpm', 'Schedule').value('NA'),
        plugins: ['sortbutton'],
        width: 170,
        fixed: true,
        dataIndex: 'Name',
        menuDisabled: true,
        cls: 'sch-event-header-na',
        items: [{
            xtype: 'schedulefilterfield',
            margin: '0 10 10 10',
            property: 'Name',
            flex: 1
        }],
        // prevent sortchange on Enter
        onEnterKey: function () {
        },
    }],

    resourceStore: {
        type: 'resourcestore',
        storeId: 'MyResources',
        idProperty: 'Id',
        model: 'App.model.tpm.baseclient.BaseClient',
        autoLoad: true
        //sortInfo: { field: 'Id', direction: 'ASC' }
    },

    eventStore: {
        type: 'schedulepromostore',
        extendedFilter: {
            xclass: 'App.ExtFilterContext',
            supportedModels: [{
                xclass: 'App.ExtSelectionFilterModel',
                model: 'App.model.tpm.promo.PromoView',
                modelId: 'efselectionmodel'
            }, {
                xclass: 'App.ExtTextFilterModel',
                modelId: 'eftextmodel'
            }]
        },
        fixedFilters: {
            'statusfilter': {
                property: 'PromoStatusSystemName',
                operation: 'NotEqual',
                value: 'Deleted'
            }
        },
    },

    promoStore: Ext.create('App.store.core.SimpleStore', {
        model: 'App.model.tpm.promo.Promo',
        storeId: 'schedulerpromostore',
        autoLoad: false,
    }),

    initComponent: function () {
        var me = this;
        Ext.apply(this, {
            // Custom time axis by Mars Dates
            timeAxis: new App.view.tpm.schedule.MarsTimeAxis()
        });
        this.callParent();
    },

    reloadEventsStore: function () {
        var me = this;
        me.setLoading('Loading promoes');
        var store = me.getEventStore();
        store.on('load', function (store, records, success) {
            me.setLoading(false);
        });
        store.load();
    },

    onDestroy: function () {
        var system = Ext.ComponentQuery.query('system')[0];
        if (system.tabBar.activeTab) {
            system.tabBar.activeTab.deactivate();
            system.tabBar.activeTab = null;
            system.activeTab = null;
            system.collapse();
        }
        var notify = Ext.ComponentQuery.query('[cls=ux-notification-light]')[0]
        if (notify != null) {
            notify.close();
        }
        system.remove('promoDetailTab');
        this.callParent();
    }
});
