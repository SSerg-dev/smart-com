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
    loadMask: false,
    rowHeight: 21,
    clientsFilterConfig: [],
    
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
                if (date > new Date() || App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
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
    tooltipTpl: new Ext.XTemplate(
        '<tpl if="TypeName == \'Competitor\'">' +
            '<dl class="eventTip">' +
            '<dt>Name</dt><dd>{Name}</dd>' +
            '<dt>BrandTech</dt><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;">{CompetitorBrandTechName}</dd>' +
            '<dt><span class="mdi inout-mark-icon">&#x{TypeGlyph}</span>{TypeName} promo</dt>' +
            '</dl>' +
        '<tpl else>' +
            '<dl class="eventTip">' +
            '<dt>Name</dt><dd>{Name}</dd>' +
            '<dt>Mechanic</dt><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;">{MarsMechanicName} <tpl if="MarsMechanicName === \'TPR\' || MarsMechanicName === \'Other\'">{MarsMechanicDiscount}%</tpl><tpl if="MarsMechanicName === \'VP\'">{MarsMechanicTypeName}</tpl></dd>' +
            '<dl style="border-left: 5px solid {PromoStatusColor}; margin: 0px;"><dt style="margin: 0 0 3px 2px;">Status</dt><dd style="margin: 0 0 3px 2px;">{PromoStatusSystemName}</dd></dl>' +
            '<tpl if="TypeName == \'InOut\'"><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;"></dd>' +
            '<dt><span class="mdi mdi-hexagon-slice-2 inout-mark-icon"></span>InOut promo</dt></tpl>' +
            '<tpl if="TypeName != \'InOut\' && TypeName != \'Regular\'"><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;"></dd>' +
            '<dt><span class="mdi inout-mark-icon">&#x{TypeGlyph}</span>{TypeName} promo</dt></tpl>' +
            '<tpl if="IsGrowthAcceleration"><dd style="border-bottom: 1px solid rgba(197, 197, 197, 0.25); padding-bottom: 5px;"></dd>' +
            '<dt><span class="mdi alpha-g-box-outline inout-mark-icon"></span>Growth Acceleration</dt></tpl>' + 
            '<dt style="border-top: 1px solid rgba(197, 197, 197, 0.25); padding-top: 5px;">Invoice Type</dt><dd>{IsOnInvoice}</dd>' +
            '</dl>' +
        '</tpl>' 
    ),
    tipCfg: {
        cls: 'sch-tip',
        height: 'auto',
        showDelay: 200,
        hideDelay: 200,
        autoHide: true,
		anchor: 'b',
        show: function () { // если высота меньше чем высота подсказки, подсказка начинает мерцать - баг в функции show плагина
			Ext.ToolTip.prototype.show.apply(this, arguments);
        }
    },
    // Промо в две строки
    eventBodyTemplate: '<span class="mdi alpha-g-box-outline inout-mark-icon growth-acceleration-icon" style="display: {display}; bacground-color: white; color: #426a97; float: left;"></span>' + 
        '<div class="event-status-border {eventStatusBorderGrowthAcceleration}" style="margin-left: {marginLeft}; border-left: 5px solid {statusColor};"><span class="sch-event-text">{headerText}</span></div>',

    // возвращает объект заполняющий eventBodyTemplate
    eventRenderer: function (event, resource, tplData, row, col, ds) {
        var bgColor = event.get('ColorSystemName');
        tplData.style = Ext.String.format('background: {0} !important; border-color: white;', bgColor || '#DDDDDD');
        return {
            headerText: event.get('Name'),
            statusColor: event.get('PromoStatusColor') || '#DDDDDD',
            display: event.data.IsGrowthAcceleration ? 'inline' : 'none',
            marginLeft: event.data.IsGrowthAcceleration ? '13px' : '0px',
            eventStatusBorderGrowthAcceleration:  event.data.IsGrowthAcceleration ? 'event-status-border-growth-acceleration' : ''
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
            xtype: 'label',
            itemId: 'clientsPromoTypeFilterLabel',
            height: 30,
            text: l10n.ns('tpm', 'Schedule').value('AllSelected'),
            cls: 'ClientTypeFilterButton',
        //}, {
        //    xtype: 'schedulefilterfield',
        //    margin: '0 10 10 10',
        //    property: 'Name',
        //    flex: 1
        }],
        xtype: 'templatecolumn',

        tpl: new Ext.XTemplate(
            '<div style="font-weight: 600">{Name}</div>',
            '<tpl if="TypeName == \'InOut\'">',
            '<div class="inout-mark-text"><span class="mdi mdi-hexagon-slice-2 inout-mark-icon"></span>InOut promo</div>',
            '<tpl elseif="TypeName == \'Regular\'">',
            '<div class="inout-mark-text">Regular promo</div>',
            '<tpl elseif="TypeName == \'Competitor\'">',
            '<div class="inout-mark-text">{CompetitorName}</div>',
            '<tpl else>',
            '<div class="inout-mark-text"><span class="mdi mdi-tag-multiple-2 inout-mark-icon"></span>Other promo</div>',
            '</tpl>'
        ),


        // prevent sortchange on Enter
        onEnterKey: function () {
        },
    }],

    resourceStore: {
        type: 'resourcestore',
        storeId: 'MyResources',
        idProperty: 'InOutId',
        model: 'App.model.tpm.baseclient.SchedulerClientTreeDTO',
        autoLoad: true,
        pageSize: 100,
        //sortInfo: { field: 'Id', direction: 'ASC' }
    },

    eventStore: {
        type: 'schedulepromostore',
        storeId: 'eventStore',
        autoLoad: false,
        extendedFilter: {
            xclass: 'App.ExtFilterContext',
            //Оверрайдим метод, т.к. загрузка стора внутри этого метода ломает загрузку
            commit: function (suppressReload) {
                var model = this.getFilterModel();

                if (!model) {
                    return;
                }

                model.commit();
                this.filter = model.getFilter();
                //this.reloadStore(suppressReload);

                var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                scheduler.filter = JSON.parse(JSON.stringify(this.filter));
                this.fireEvent('extfilterchange', this);
            },
            clear: function (suppressReload) {
                var model = this.getFilterModel();

                if (!model) {
                    return;
                }

                model.clear();
                this.filter = model.getFilter();
                //this.reloadStore(suppressReload);
                this.fireEvent('extfilterchange', this);
            },
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
        reload: function () {
            var cntr = App.app.getController('tpm.schedule.SchedulerViewController');
            cntr.eventStoreLoading(this);
        },
    },

    promoStore: Ext.create('App.store.core.SimpleStore', {
        model: 'App.model.tpm.promo.Promo',
        storeId: 'schedulerpromostore',
        autoLoad: false,
    }),

    competitorPromoStore: Ext.create('App.store.core.SimpleStore', {
        model: 'App.model.tpm.competitorpromo.CompetitorPromo',
        storeId: 'schedulercompetitorpromostore',
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

    //Не используется
    //reloadEventsStore: function () {
    //    var me = this;
    //    me.setLoading('Loading promoes');
    //    var store = me.getEventStore();
    //    store.on('load', function (store, records, success) {
    //        me.setLoading(false);
    //    });
    //    store.load();
    //},

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
