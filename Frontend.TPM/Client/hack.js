if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.indexOf(searchString, position) === position;
    };
}

Math.sign = Math.sign || function (x) {
    x = +x;
    if (x === 0 || isNaN(x)) {
        return x;
    }
    return x > 0 ? 1 : -1;
}

if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        if (this == null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

Ext.override(Ext.data.Connection, {
    setupUrl: function (options, url) {
        var form = this.getForm(options);
        if (form) {
            url = url || form.action;
        }
        var separator = location.href[location.href.length - 1] === '/' ? '' : '/';
        url = location.href + separator + url;
        return url;
    }
});

Ext.override(Ext, {
    getScrollbarSize: function (force) {
        return {
            width: 21,
            height: 21
        };
    }
});

Ext.override(Ext.form.field.Base, {
    fieldLabelTip: null,
    fieldValueTip: null,

    /**
     * Override of isValid, to only validate if we are not read-only.
     * @return {Boolean}
     */
    isValid: function () {
        var me = this,
              readOnly = me.readOnly,
              validate = me.forceValidation || !readOnly;

        if (validate) {
            return me.callParent(arguments);
        } else {
            if (!me.preventMark) {
                if (readOnly) {
                    me.clearInvalid();
                }
            }

            return readOnly;
        }
    },

    /**
     * Override to add tooltip
     */
    afterFirstLayout: function () {
        var me = this;
        this.callParent(arguments);

        if (me.labelEl) {
            me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                target: me.labelEl,
                preventLoseFocus: true,
                trackMouse: true,
                html: me.getFieldLabel()
            });
        }
        var isLogWindow = me.xtype == 'textarea' || me.xtype == 'textareafield' && me.name == 'logtext';
        if (me.inputEl && !isLogWindow) {
            me.fieldValueTip = Ext.create('Ext.tip.ToolTip', {
                target: me.inputEl,
                preventLoseFocus: true,
                trackMouse: true,
                listeners: {
                    beforeshow: function (tip) {
                        var value = me.getDisplayValue ? me.getDisplayValue() : me.getRawValue();
                        tip.update(String(value));
                        return !Ext.isEmpty(value);
                    },
                }
            });
        }
    }
});

Ext.override(Ext.FocusManager, {
    /* При скрытии подсказки поле редактирования не должно терять фокус */
    onComponentHide: function (cmp) {
        var me = this,
            cmpHadFocus = false,
            focusedCmp = me.focusedCmp,
            parent;

        if (focusedCmp) {


            cmpHadFocus = cmp.hasFocus || (cmp.isContainer && cmp.isAncestor(me.focusedCmp));
        }

        me.clearComponent(cmp);


        if (cmpHadFocus && (parent = cmp.up(':focusable'))) {
            parent.focus();
        } else if (!cmp.preventLoseFocus && cmp.componentCls != "x-tip") { //дополнительная проверка для хинтов от textfield (пустое поле) которые почему то преобразуются в другой компонент и теряют свойство preventLoseFocus
            me.focusEl.focus();
        }
    },
});

Ext.override(Ext.form.field.Field, {
    /**
     * Override of isValid, to only validate if we are not read-only.
     * @return {Boolean}
     */
    isValid: function () {
        var me = this;
        return me.readOnly || me.callParent(arguments);
    }
});

Ext.override(Ext.form.field.Text, {
    getErrors: function (value) {
        var me = this,
            errors = [],
            validator = me.validator,
            vtype = me.vtype,
            vtypes = Ext.form.field.VTypes,
            regex = me.regex,
            format = Ext.String.format,
            msg, trimmed, isBlank;

        value = value || me.processRawValue(me.getRawValue());

        if (Ext.isFunction(validator)) {
            msg = validator.call(me, value);
            if (msg !== true) {
                errors.push(msg);
            }
        }


        trimmed = (typeof value != 'string') ? value : Ext.String.trim(value);

        if (trimmed.length < 1 || (value === me.emptyText && me.valueContainsPlaceholder)) {
            if (!me.allowBlank) {
                errors.push(me.blankText);
            }

            if (!me.validateBlank) {
                return errors;
            }
            isBlank = true;
        }



        if (!isBlank && value.length < me.minLength) {
            errors.push(format(me.minLengthText, me.minLength));
        }

        if (value.length > me.maxLength) {
            errors.push(format(me.maxLengthText, me.maxLength));
        }

        if (vtype) {
            if (!vtypes[vtype](value, me)) {
                errors.push(me.vtypeText || vtypes[vtype + 'Text']);
            }
        }

        if (regex && !regex.test(value)) {
            errors.push(me.regexText || me.invalidText);
        }

        return errors;
    }
});
var isExplorer = function () {
    return "onwheel" in document.createElement("div") ? false : true;
}

Ext.override(Ext.Container, {

    initComponent: function () {
        this.callParent(arguments);

        if (this._isScrollable()) {
            this.on({
                resize: this.onPanelResize,
                afterlayout: this.onPanelAfterLayout,
                scope: this
            });
        }
    },

    onPanelResize: function (panel) {
        this._refreshScroll(panel);
        panel.doLayout();
    },

    onPanelAfterLayout: function (panel) {
        //Ломает скролл на большом экране
        if (!panel.stopRefreshScroll) {
            this._refreshScroll(panel);
        }
    },

    _isScrollable: function () {
        return this.autoScroll
            || this.overflowX == 'auto'
            || this.overflowX == 'scroll'
            || this.overflowY == 'auto'
            || this.overflowY == 'scroll';
    },

    _refreshScroll: function (panel) {
        var targetEl = panel.getTargetEl();

        if (!targetEl) {
            return;
        }

        var domEl = $(targetEl.dom);

        if (!panel.hasScroll) {
            //TODO: могут быть проблемы в панелях, у которых есть header или dockedItems
            domEl.jScrollPane({
                verticalGutter: 10,
                horizontalGutter: 10,
                contentWidth: !panel.overflowX || panel.overflowX == 'hidden' ? 0 : undefined
            });

            domEl.bind(
				'jsp-initialised',
				function (event, isScrollable) {
				    var jspData = domEl.data('jsp');

				    if (jspData.getIsScrollableV()) {
				        domEl.addClass('has-vertical-scroll');
				    } else {
				        domEl.removeClass('has-vertical-scroll');
				    }

				    if (jspData.getIsScrollableH()) {
				        domEl.addClass('has-horizontal-scroll');
				    } else {
				        domEl.removeClass('has-horizontal-scroll');
				    }
				}
            );

            domEl.bind(
                'jsp-scroll-x',
                function (event, scrollPositionX, isAtLeft, isAtRight) {
                    if (!Ext.isNumber(scrollPositionX)) return;

                    if (isExplorer()) {
                        var event = document.createEvent('WheelEvent', { "deltax": scrollPositionX, "deltamode": 0 });
                        event.initEvent('scroll', true, true)
                        targetEl.dom.dispatchEvent(event);
                    } else {
                        targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                            deltaX: scrollPositionX,
                            deltaMode: 0
                        }));
                    }
                }
            );

            domEl.bind(
                'jsp-scroll-y',
                function (event, scrollPositionY, isAtTop, isAtBottom) {
                    var menuItems = panel.query('menuitem[hidden=false]');
                    var shownMenuItems = [];
                    for (var i = 0; i < menuItems.length; i++) {
                        if (menuItems[i].getTargetEl() != null)
                            shownMenuItems.push(menuItems[i]);
                    }

                    if (isExplorer()) {
                        var event = document.createEvent('WheelEvent', { "deltay": 4, "deltamode": 0 });
                        event.initEvent('scroll', true, true)
                        targetEl.dom.dispatchEvent(event);
                    }
                    else {


                        for (var i = 0; i < shownMenuItems.length; i++) {
                            var mainPanel = shownMenuItems[i].up('combineddirectorypanel');
                            if (mainPanel != null) {
                                var headerHeight = mainPanel.down('header').getHeight();
                                var currentParentMenu = null;
                                var menuPanel = shownMenuItems[i].parentMenu;
                                if (menuPanel == currentParentMenu) continue;
                                if (menuPanel != null) {
                                    menuPanel.setY(mainPanel.getY() + headerHeight);
                                    menuPanel.el.setStyle('zIndex', 1);
                                    menuPanel.el.shadow.zIndex = 1;
                                    currentParentMenu = menuPanel;
                                }
                            }
                        }
                        if (Ext.isNumber(scrollPositionY))
                            targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                                deltaY: scrollPositionY,
                                deltaMode: 0
                            }));
                    }
                }
            );

            panel.hasScroll = true;
        } else if (panel.hasScroll) {
            domEl.data('jsp').reinitialise();
        }
    }
});

Ext.override(Ext.grid.ColumnLayout, {
    getContainerSize: function (ownerContext) {
        if (this.grid && this.grid.getView().hasScroll) {
            var me = this,
                result,
            viewContext = ownerContext.viewContext,
            viewHeight;

            // Column, NOT the main grid's HeaderContainer
            if (me.owner.isColumn) {
                result = me.getColumnContainerSize(ownerContext);
            }

                // This is the maingrid's HeaderContainer
            else {
                result = me.callParent(arguments);

                // If we've collected a viewContext and we're not shrinkwrapping the height
                // then we see if we have to narrow the width slightly to account for scrollbar
                if (viewContext && !viewContext.heightModel.shrinkWrap &&
                        viewContext.target.componentLayout.ownerContext) { // if (its layout is running)
                    viewHeight = viewContext.getProp('height');
                    if (isNaN(viewHeight)) {
                        me.done = false;
                    } else {
                        if (ownerContext.state.tableHeight <= viewHeight) {
                            result.width -= me.scrollbarWidth;
                        }
                        ownerContext.state.parallelDone = false;
                        viewContext.invalidate();
                    }
                }
            }

            // TODO - flip the initial assumption to "we have a vscroll" to avoid the invalidate in most
            // cases (and the expensive ones to boot)

            return result;
        } else {
            return this.callParent(arguments);
        }
    }
});

Ext.override(Ext.panel.Table, {
    initComponent: function () {
        this.callParent(arguments);
        this.on('columnresize', this.onChangeColumnWidth, this);
    },

    onChangeColumnWidth: function (container, column, width) {
        var view = this.getView();
        if (view.hasScroll) {
            $(view.getEl().dom).data('jsp').reinitialise();
        }
    },

    onHorizontalScroll: function (event, target) {
        if (this.scroll === false) {
            this.callParent(arguments);
        } else {
            this.syncHorizontalScroll(target.scrollPositionX || target.scrollLeft);
        }
    },

    syncHorizontalScroll: function (left, setBody) {
        var me = this;

        setBody = setBody === true;
        // Only set the horizontal scroll if we've changed position,
        // so that we don't set this on vertical scrolls
        if (me.rendered && (setBody || left !== me.scrollLeftPos)) {
            me.headerCt.el.dom.scrollLeft = left;
            me.scrollLeftPos = left;
        }
    },

    delayScroll: function () {
        var view = this.getView();
        if (view.hasScroll) {
            this.scrollTask.delay(10, null, null, [$(view.getEl().dom).data('jsp').getContentPositionX()]);
        } else {
            this.callParent(arguments);
        }
    },

    destroy: function () {
        var me = this;
        if (me.scroll !== false) {
            if (me.getView().getTargetEl) {
                me.getView().getTargetEl().un('scroll', me.onHorizontalScroll, me);
            }
        }
        me.callParent();
    }
});

Ext.override(Ext.view.Table, {
    constructor: function () {
        this.callParent(arguments);

        this.addEvents(
            'addhorizontalscroll',
            'addverticalscroll'
        );
    },

    refresh: function () {
        var isScheduler = this.xtype == "schedulergridview" || this.up('scheduler');
        if (this.panel.scroll === false || !this.getTargetEl() || this.xtype === "treeview" || isScheduler) {
            this.callParent(arguments);
            return;
        }

        if (!this.hasScroll) {
            var me = this,
                grid = me.panel,
                scrollContainer = $(me.getTargetEl().dom).jScrollPane({
                    verticalGutter: 0,
                    horizontalGutter: 0,
                    alwaysShowScroll: {
                        vertical: this.overflowX || false,
                        horizontal: this.overflowY || false
                    }
                }),
                jspData = scrollContainer.data('jsp'),
                targetEl = me.getTargetEl().dom,
                el = $(me.getEl().dom);

            this.hasScroll = true;

            el.bind(
				'jsp-initialised',
				function (event, isScrollable) {
				    var jspContainer = me.getEl(),
                        verticalBar = jspContainer.down('.jspVerticalBar'),
				        horizontalBar = jspContainer.down('.jspHorizontalBar'),
				        verticalTrack = verticalBar ? verticalBar.down('.jspTrack') : null,
				        horizontalTrack = horizontalBar ? horizontalBar.down('.jspTrack') : null;

				    if (verticalTrack) {
				        if (!verticalTrack.down('.jspTrackInner')) {
				            $(verticalTrack.dom).wrapInner('<div class="jspTrackInner"></div>');
				        }

				        if (!jspData.getIsScrollableV()) {
				            verticalTrack.hide();
				        } else {
				            verticalTrack.show();
				            me.fireEvent('addverticalscroll', me);
				        }
				    }

				    if (horizontalTrack) {
				        if (!horizontalTrack.down('.jspTrackInner')) {
				            $(horizontalTrack.dom).wrapInner('<div class="jspTrackInner"></div>');
				        }

				        if (!jspData.getIsScrollableH()) {
				            horizontalTrack.hide();
				            targetEl.style.left = 0;
				            grid.scrollLeftPos = 0;
				        } else {
				            horizontalTrack.show();
				            me.fireEvent('addhorizontalscroll', me);
				        }
				    }
				}
            );

            el.bind(
                'jsp-scroll-x',
                function (event, scrollPositionX, isAtLeft, isAtRight) {
                    if (!Ext.isNumber(scrollPositionX)) {
                        return;
                    }

                    targetEl.scrollPositionX = scrollPositionX;
                    if (isExplorer()) {
                        var event = document.createEvent('MouseWheelEvent', { "deltax": Math.round(scrollPositionX), "deltamode": 0 });
                        event.initEvent('scroll', true, true)
                        targetEl.dispatchEvent(event);
                    }
                    else {
                        targetEl.dispatchEvent(new WheelEvent('scroll', {
                            deltaX: Math.round(scrollPositionX),
                            deltaMode: 0
                        }));
                    }
                }
            );

            el.bind(
                'jsp-scroll-y',
                function (event, scrollPositionY, isAtTop, isAtBottom) {
                    if (!Ext.isNumber(scrollPositionY)) {
                        return;
                    }

                    targetEl.scrollPositionY = scrollPositionY;
                    if (isExplorer()) {
                        var event = document.createEvent('MouseWheelEvent', { "deltay": Math.round(scrollPositionY), "deltamode": 0 });
                        event.initEvent('scroll', true, true)
                        targetEl.dispatchEvent(event);
                    }
                    else {
                        targetEl.dispatchEvent(new WheelEvent('scroll', {
                            deltaY: Math.round(scrollPositionY),
                            deltaMode: 0
                        }));
                    }
                }
            );

            if (!grid.hideHeaders) {
                me.un('scroll', grid.onHorizontalScroll, grid);
                me.on('scroll', grid.onHorizontalScroll, grid, { element: 'jspContainer' });
            }

            if (grid.verticalScroller) {
                me.on('scroll', grid.verticalScroller.onViewScroll, grid.verticalScroller, { element: 'jspContainer' });
            }
        }

        this.callParent(arguments);

        if (this.getTargetEl().getHeight() == 0) {
            this.filler = this.body.createChild({
                style: {
                    width: '1px',
                    height: '1px'
                }
            });
        } else if (this.filler) {
            this.filler.remove();
        }

        this._refreshScroll();
    },

    getTargetEl: function () {
        var me = this;

        if (me.jspContainer) {
            return me.jspContainer;
        }

        var el = me.getEl(),
            targetEl = el ? el.down('.jspContainer > .jspPane') : null;

        if (targetEl) {
            me.jspContainer = targetEl;
            return me.jspContainer;
        } else {
            return el;
        }
    },

    refreshSize: function () {
        var me = this,
            grid,
            bodySelector = me.getBodySelector(),
            isScheduler = me.xtype == "schedulergridview" || me.up('scheduler');
        if (this.xtype === "treeview" || isScheduler) return;

        // On every update of the layout system due to data update, capture the view's main element in our private flyweight.
        // IF there *is* a main element. Some TplFactories emit naked rows.
        if (bodySelector) {
            me.body.attach(me.getTargetEl().child(bodySelector, true));
        }

        if (!me.hasLoadingHeight) {
            grid = me.up('tablepanel');

            // Suspend layouts in case the superclass requests a layout. We might too, so they
            // must be coalescsed.
            Ext.suspendLayouts();

            me.callSuper();

            // Since columns and tables are not sized by generated CSS rules any more, EVERY table refresh
            // has to be followed by a layout to ensure correct table and column sizing.
            grid.updateLayout();

            //this._refreshScroll(); // сбивается горизонтальная прокрутка при сортировке и фильтрации в гриде
            Ext.resumeLayouts(true);
        }
    },

    scrollRowIntoView: function (row) {
        row = this.getNode(row, true);

        if (row) {
            if (this.hasScroll) {
                var jspData = $(this.getEl().dom).data('jsp'),
                    scrollLeft = jspData.getContentPositionX();

                jspData.scrollToElement(row);
                jspData.scrollToX(scrollLeft);

                if (this.indexOf(row) == 0) {
                    jspData.positionDragY(0);
                }
            } else {
                Ext.fly(row).scrollIntoView(this.el, false);
            }
        }
    },

    onResize: function () {
        this.callParent(arguments);
        this._refreshScroll();
    },

    _refreshScroll: function () {
        var me = this,
            el = me.getEl(),
            isScheduler = me.xtype == "schedulergridview" || me.up('scheduler');

        if (me.hasScroll && me.xtype != "treeview" && !isScheduler && me.panel.isVisible()) {
            $(el.dom).data('jsp').reinitialise();
            if (!el.down('.jspVerticalBar') || !el.down('.jspVerticalBar > .jspTrack').isVisible()) {
                me.getTargetEl().dom.style.top = 0;
            }
        }
    }
});

Ext.override(Ext.grid.plugin.BufferedRenderer, {
    // Initialize this as a plugin
    init: function (grid) {
        var me = this,
            view = grid.view,
            viewListeners = {
                boxready: me.onViewResize,
                resize: me.onViewResize,
                refresh: me.onViewRefresh,
                scope: me,
                destroyable: true
            };

        // If we are using default row heights, then do not sync row heights for efficiency
        if (!me.variableRowHeight && grid.ownerLockable) {
            grid.ownerLockable.syncRowHeight = false;
        }

        // If we are going to be handling a NodeStore then it's driven by node addition and removal, *not* refreshing.
        // The view overrides required above change the view's onAdd and onRemove behaviour to call onDataRefresh when necessary.
        if (grid.isTree || grid.ownerLockable && grid.ownerLockable.isTree) {
            view.blockRefresh = false;
            view.loadMask = true;
        }
        if (view.positionBody) {
            viewListeners.refresh = me.onViewRefresh;
        }
        me.grid = grid;
        me.view = view;
        view.bufferedRenderer = me;
        view.preserveScrollOnRefresh = true;

        me.bindStore(view.dataSource);
        view.getViewRange = function () {
            return me.getViewRange() || [];
        };

        me.position = 0;

        me.gridListeners = grid.on('reconfigure', me.onReconfigure, me);
        me.viewListeners = view.on(viewListeners);
    },

    onStoreClear: function () {
        var me = this,
            targetEl = me.view.getTargetEl();

        // Do not do anything if view is not rendered, or if the reason for cache clearing is store destruction
        if (me.view.rendered && !me.store.isDestroyed) {
            // Temporarily disable scroll monitoring until the scroll event caused by any following *change* of scrollTop has fired.
            // Otherwise it will attempt to process a scroll on a stale view
            if (me.scrollTop !== 0) {
                me.ignoreNextScrollEvent = true;
                targetEl.dom.scrollTop = 0;
                targetEl.dom.style.top = 0;
                targetEl.dom.scrollPositionY = 0;
            }

            me.bodyTop = me.scrollTop = me.position = me.scrollHeight = 0;
            me.lastScrollDirection = me.scrollOffset = null;

            // MUST delete, not null out because the calculation checks hasOwnProperty
            delete me.rowHeight;
        }
    },

    onViewRefresh: function () {
        var me = this,
            view = me.view,
            oldScrollHeight = me.scrollHeight,
            scrollHeight,
            scrollPositionY;

        if (!view.panel.isVisible()) {
            return;
        }

        // View has rows, delete the rowHeight property to trigger a recalculation when scrollRange is calculated
        if (view.all.getCount()) {
            // We need to calculate the table size based upon the new viewport size and current row height
            // It tests hasOwnProperty so must delete the property to make it recalculate.
            delete me.rowHeight;
        }

        // Calculates scroll range. Also calculates rowHeight if we do not have an own rowHeight property.
        // That will be the case if the view contains some rows.
        scrollHeight = me.getScrollHeight();

        if (!oldScrollHeight || scrollHeight != oldScrollHeight) {
            me.stretchView(view, scrollHeight);
        }

        scrollPositionY = view.getTargetEl().dom.scrollPositionY || 0;
        if (me.scrollTop !== scrollPositionY) {
            // The view may have refreshed and scrolled to the top, for example
            // on a sort. If so, it's as if we scrolled to the top, so we'll simulate
            // it here.
            me.onViewScroll();
        } else {
            me.setBodyTop(me.bodyTop);

            // With new data, the height may have changed, so recalculate the rowHeight and viewSize.
            if (view.all.getCount()) {
                me.viewSize = 0;
                me.onViewResize(view, null, view.getHeight());
            }
        }
    },

    stretchView: function (view, scrollRange) {
        //var me = this,
        //    recordCount = (me.store.buffered ? me.store.getTotalCount() : me.store.getCount());

        // If this is the last page, correct the scroll range to be just enough to fit.
        //if (recordCount && (me.view.all.endIndex === recordCount - 1)) {
        //    scrollRange = me.bodyTop + view.body.dom.offsetHeight;
        //}

        view.getTargetEl().dom.style.height = scrollRange + 'px';
    },

    setViewSize: function (viewSize) {
        if (viewSize !== this.viewSize) {

            // Must be set for getFirstVisibleRowIndex to work
            this.scrollTop = this.view.getTargetEl().dom.scrollPositionY || 0;

            var me = this,
                store = me.store,
                elCount = me.view.all.getCount(),
                start, end,
                lockingPartner = me.lockingPartner;

            me.viewSize = store.viewSize = viewSize;

            // If a store loads before we have calculated a viewSize, it loads me.defaultViewSize records.
            // This may be larger or smaller than the final viewSize so the store needs adjusting when the view size is calculated.
            if (elCount) {
                start = me.view.all.startIndex;
                end = Math.min(start + viewSize - 1, (store.buffered ? store.getTotalCount() : store.getCount()) - 1);

                // While rerendering our range, the locking partner must not sync
                if (lockingPartner) {
                    lockingPartner.disable();
                }
                me.renderRange(start, end);
                if (lockingPartner) {
                    lockingPartner.enable();
                }
            }
        }
        // Workaround: Для Chrome - Если экспортировать файл в нижней части страницы появится область с информацией о загрузке
        // Пока эта область не закрыта, при загрузке грида store.get(0, viewSize - 1 (обычно 50) ) возвращает 0, если в справочнике больше 1-й страницы записей
        // В результате - грид пустой, если прокрутить вниз - записи подгружаются.
        // принудительно уменьшаем viewSize на 1 для корректного отображения записей
        return viewSize - 1;
    },

    /**
     * Scrolls to and optionlly selects the specified row index **in the total dataset**.
     * 
     * @param {Number} recordIdx The zero-based position in the dataset to scroll to.
     * @param {Boolean} doSelect Pass as `true` to select the specified row.
     * @param {Function} callback A function to call when the row has been scrolled to.
     * @param {Number} callback.recordIdx The resulting record index (may have changed if the passed index was outside the valid range).
     * @param {Ext.data.Model} callback.record The resulting record from the store.
     * @param {Object} scope The scope (`this` reference) in which to execute the callback. Defaults to this BufferedRenderer.
     * 
     */
    scrollTo: function (recordIdx, doSelect, callback, scope) {
        var me = this,
            view = me.view,
            viewDom = view.getEl().dom,
            store = me.store,
            total = store.buffered ? store.getTotalCount() : store.getCount(),
            startIdx, endIdx,
            targetRec,
            targetRow,
            tableTop,
            groupingFeature,
            group,
            record;

        // If we have a grouping summary feature rendering the view in groups,
        // first, ensure that the record's group is expanded,
        // then work out which record in the groupStore the record is at.
        if ((groupingFeature = view.dataSource.groupingFeature) && (groupingFeature.collapsible !== false)) {

            // Sanitize the requested record
            recordIdx = Math.min(Math.max(recordIdx, 0), view.store.getCount() - 1);
            record = view.store.getAt(recordIdx);
            recordIdx = groupingFeature.indexOf(recordIdx);
            group = groupingFeature.getGroup(record);

            if (group.isCollapsed) {
                groupingFeature.expand(group.name);
                total = store.buffered ? store.getTotalCount() : store.getCount();
            }

        } else {

            // Sanitize the requested record
            recordIdx = Math.min(Math.max(recordIdx, 0), total - 1);
        }

        // Calculate view start index
        startIdx = Math.max(Math.min(recordIdx - ((me.leadingBufferZone + me.trailingBufferZone) / 2), total - me.viewSize + 1), 0);
        tableTop = startIdx * me.rowHeight;
        endIdx = Math.min(startIdx + me.viewSize - 1, total - 1);

        store.getRange(startIdx, endIdx, {
            callback: function (range, start, end) {

                me.renderRange(start, end, true);

                targetRec = store.data.getRange(recordIdx, recordIdx)[0];
                targetRow = view.getNode(targetRec, false);
                //view.body.dom.style.top = tableTop + 'px';

                me.position = me.scrollTop = viewDom.scrollTop = tableTop = Math.min(Math.max(0, tableTop - view.body.getOffsetsTo(targetRow)[1]), view.getTargetEl().dom.scrollHeight - viewDom.clientHeight);

                // https://sencha.jira.com/browse/EXTJSIV-7166 IE 6, 7 and 8 won't scroll all the way down first time
                if (Ext.isIE) {
                    viewDom.scrollTop = tableTop;
                }
                if (doSelect) {
                    view.selModel.select(targetRec);
                }

                view.scrollRowIntoView(targetRow);
                //workaround При создании записи не пересчитывается прокрутка
                me.onRangeFetched(range, start, end, false);
                view._refreshScroll();

                if (callback) {
                    callback.call(scope || me, recordIdx, targetRec);
                }
            }
        });
    },

    onViewScroll: function (e, t) {
        var me = this,
            store = me.store,
            totalCount = (store.buffered ? store.getTotalCount() : store.getCount()),
            vscrollDistance,
            scrollDirection,
            scrollTop = me.scrollTop = me.view.getTargetEl().dom.scrollPositionY || 0,
            scrollHandled = false;

        // Flag set when the scrollTop is programatically set to zero upon cache clear.
        // We must not attempt to process that as a scroll event.
        if (me.ignoreNextScrollEvent) {
            me.ignoreNextScrollEvent = false;
            return;
        }

        // Only check for nearing the edge if we are enabled, and if there is overflow beyond our view bounds.
        // If there is no paging to be done (Store's dataset is all in memory) we will be disabled.
        if (!(me.disabled || totalCount < me.viewSize)) {

            vscrollDistance = scrollTop - me.position;
            scrollDirection = vscrollDistance > 0 ? 1 : -1;

            // Moved at leat 20 pixels, or cvhanged direction, so test whether the numFromEdge is triggered
            if (Math.abs(vscrollDistance) >= 20 || (scrollDirection !== me.lastScrollDirection)) {
                me.lastScrollDirection = scrollDirection;
                me.handleViewScroll(me.lastScrollDirection);
                scrollHandled = true;
            }
        }

        // Keep other side synced immediately if there was no rendering work to do.
        if (!scrollHandled) {
            if (me.lockingPartner && me.lockingPartner.scrollTop !== scrollTop) {
                me.lockingPartner.view.getTargetEl().dom.scrollTop = scrollTop;
            }
        }
    },

    handleViewScroll: function (direction) {
        var me = this,
            rows = me.view.all,
            store = me.store,
            viewSize = me.viewSize,
            totalCount = (store.buffered ? store.getTotalCount() : store.getCount()),
            requestStart,
            requestEnd;

        // We're scrolling up
        if (direction == -1) {

            // If table starts at record zero, we have nothing to do
            if (rows.startIndex) {
                if ((me.getFirstVisibleRowIndex() - rows.startIndex) < me.numFromEdge) {
                    requestStart = Math.max(0, me.getLastVisibleRowIndex() + me.trailingBufferZone - viewSize);
                }
            }
        }
            // We're scrolling down
        else {

            // If table ends at last record, we have nothing to do
            if (rows.endIndex < totalCount - 1) {
                if ((rows.endIndex - me.getLastVisibleRowIndex()) < me.numFromEdge) {
                    requestStart = Math.max(0, me.getFirstVisibleRowIndex() - me.trailingBufferZone);
                }
            }
        }

        // We scrolled close to the edge and the Store needs reloading
        if (requestStart != null) {
            requestEnd = Math.min(requestStart + viewSize - 1, totalCount - 1);

            // If calculated view range has moved, then render it
            if (requestStart !== rows.startIndex || requestEnd !== rows.endIndex) {
                me.renderRange(requestStart, requestEnd);
                return;
            }
        }

        // If we did not have to render, then just sync the partner's scroll position
        if (me.lockingPartner && me.lockingPartner.view.getTargetEl() && me.lockingPartner.scrollTop !== me.scrollTop) {
            me.lockingPartner.view.getTargetEl().dom.scrollTop = me.scrollTop;
        }
    },

    onRangeFetched: function (range, start, end, fromLockingPartner) {
        var me = this,
            view = me.view,
            oldStart,
            rows = view.all,
            removeCount,
            increment = 0,
            calculatedTop = start * me.rowHeight,
            top,
            lockingPartner = me.lockingPartner;

        // View may have been destroyed since the DelayedTask was kicked off.
        if (view.isDestroyed) {
            return;
        }

        // If called as a callback from the Store, the range will be passed, if called from renderRange, it won't
        if (!range) {
            range = me.store.getRange(start, end);

            // Store may have been cleared since the DelayedTask was kicked off.
            if (!range) {
                return;
            }
        }

        // No overlapping nodes, we'll need to render the whole range
        if (start > rows.endIndex || end < rows.startIndex) {
            rows.clear(true);
            top = calculatedTop;
        }

        if (!rows.getCount()) {
            view.doAdd(range, start);
        }
            // Moved down the dataset (content moved up): remove rows from top, add to end
        else if (end > rows.endIndex) {
            removeCount = Math.max(start - rows.startIndex, 0);

            // We only have to bump the table down by the height of removed rows if rows are not a standard size
            if (me.variableRowHeight) {
                increment = rows.item(rows.startIndex + removeCount, true).offsetTop;
            }
            rows.scroll(Ext.Array.slice(range, rows.endIndex + 1 - start), 1, removeCount, start, end);

            // We only have to bump the table down by the height of removed rows if rows are not a standard size
            if (me.variableRowHeight) {
                // Bump the table downwards by the height scraped off the top
                top = me.bodyTop + increment;
            } else {
                top = calculatedTop;
            }
        }
            // Moved up the dataset: remove rows from end, add to top
        else {
            removeCount = Math.max(rows.endIndex - end, 0);
            oldStart = rows.startIndex;
            rows.scroll(Ext.Array.slice(range, 0, rows.startIndex - start), -1, removeCount, start, end);

            // We only have to bump the table up by the height of top-added rows if rows are not a standard size
            if (me.variableRowHeight) {
                // Bump the table upwards by the height added to the top
                top = me.bodyTop - rows.item(oldStart, true).offsetTop;
            } else {
                top = calculatedTop;
            }
        }
        // The position property is the scrollTop value *at which the table was last correct*
        // MUST be set at table render/adjustment time
        me.position = me.scrollTop;

        // Position the table element. top will be undefined if fixed row height, so table position will
        // be calculated.
        if (view.positionBody) {
            me.setBodyTop(top, calculatedTop);
        }

        // Sync the other side to exactly the same range from the dataset.
        // Then ensure that we are still at exactly the same scroll position.
        if (lockingPartner && !lockingPartner.disabled && !fromLockingPartner) {
            lockingPartner.onRangeFetched(range, start, end, true);
            if (lockingPartner.scrollTop !== me.scrollTop) {
                lockingPartner.view.getTargetEl().dom.scrollTop = me.scrollTop;
            }
        }
    },

    setBodyTop: function (bodyTop, calculatedTop) {
        var me = this,
            view = me.view,
            store = me.store,
            body = view.body.dom,
            delta;

        bodyTop = Math.floor(bodyTop);

        // See if there's a difference between the calculated top and the requested top.
        // This can be caused by non-standard row heights introduced by features or non-standard
        // data in rows.
        if (calculatedTop !== undefined) {
            delta = bodyTop - calculatedTop;
            bodyTop = calculatedTop;
        }
        body.style.position = 'absolute';
        body.style.top = (me.bodyTop = bodyTop) + 'px';

        // Adjust scrollTop to keep user-perceived position the same in the case of the calculated position not matching where the actual position was.
        // Set position property so that scroll handler does not fire in response.
        if (delta) {
            var scrollPositionY = view.getTargetEl().dom.scrollPositionY || 0;
            me.scrollTop = me.position = scrollPositionY -= delta;
        }

        // If this is the last page, correct the scroll range to be just enough to fit.
        if (view.all.endIndex === (store.buffered ? store.getTotalCount() : store.getCount()) - 1) {
            me.stretchView(view, me.bodyTop + body.offsetHeight);
        }
    },

    destroy: function () {
        var me = this,
            view = me.view;

        if (view && view.getTargetEl()) {
            view.getTargetEl().un('scroll', me.onViewScroll, me); // un does not understand the element options
        }

        // Remove listeners from old grid, view and store
        Ext.destroy(me.viewListeners, me.storeListeners, me.gridListeners);
    }
});

Ext.override(Ext.LoadMask, {
    maskCls: 'x-mask loadmask',

    bindComponent: function (comp) {
        this.callParent(arguments);
        this.ownerCmp = comp;
    },

    // Workaround в связи с багом в loadmask, если компонент не floating
    setZIndex: function (index) {
        var me = this,
            owner = me.activeOwner || me.ownerCmp;

        if (owner && owner.xtype !== 'viewport') {
            // it seems silly to add 1 to have it subtracted in the call below,
            // but this allows the x-mask el to have the correct z-index (same as the component)
            // so instead of directly changing the zIndexStack just get the z-index of the owner comp
            var zIndex = owner.el.getStyle('zIndex');
            index = (Ext.isNumeric(zIndex) ? parseInt(zIndex, 10) : 1) + 1;
        }

        me.getMaskEl().setStyle('zIndex', this.fixedZIndex || index - 1);
        return me.mixins.floating.setZIndex.apply(me, arguments);
    }
});

Ext.override(Ext.form.field.Date, {
    startDay: 1
});

/* from https://www.sencha.com/forum/showthread.php?171525-suspendEvents-did-not-affect-to-Ext.app.Controller.control */
/**
 * @class Ext.override.app.EventDomain
 * @override Ext.app.EventDomain
 * Override for MVC Controllers to support suspendEvents.
 */
Ext.define('Ext.override.app.EventDomain', {
    override: 'Ext.app.EventDomain',
    /**
	 * @inheritdoc
	 */
    constructor: function () {
        this.callParent(arguments);
        this.eventQueue = {};
    },
    /**
	 * @inheritdoc
	 */
    monitor: function (observable) {
        this.callParent(arguments);
        this.applyPrototypeMembers(observable);
    },
    /**
	 * Override prorotype functions
	 * @param {Ext.util.Observable/Ext.Class} observable
	 * @private
	 */
    applyPrototypeMembers: function (observable) {
        var domain = this,
			prototype = observable.isInstance ? observable : observable.prototype,
			resumeEvents = prototype.resumeEvents;

        prototype.resumeEvents = function (ev, args) {
            var ret = resumeEvents.apply(this, arguments);

            domain.resumeQueuedEvents(this);

            return ret;
        };
    },
    /**
	 * @inheritdoc
	 */
    dispatch: function (target, ev, args) {
        // don't dispatch if suspended
        if (target.eventsSuspended) {
            // Queue them when queued via target.suspendEvents(true)
            if (!!target.eventQueue) {
                target.domainEventQueue = target.domainEventQueue || [];
                target.domainEventQueue.push([target, ev, args]);
            }
            return true;
        }
        return this.callParent(arguments);
    },
    /**
	 * Called every resumeEvents on an Observable.
	 * Fire queued events if available.
	 * @private
	 */
    resumeQueuedEvents: function (target) {
        var me = this,
			queue = target.domainEventQueue,
			i = 0,
			len;
        if (queue)
            for (len = queue.length; i < len; i++)
                me.dispatch.apply(me, queue[i]);
        delete target.domainEventQueue;
    }
}, function () {
    var type,
		inst = Ext.app.EventDomain.instances,
		i,
		len;
    for (type in inst)
        if (inst.hasOwnProperty(type)) {
            inst[type].eventQueue = {};
            for (i = 0, len = inst[type].monitoredClasses.length; i < len; i++)
                inst[type].applyPrototypeMembers(inst[type].monitoredClasses[i]);
        }
});

Ext.override(Ext.grid.header.Container, {
    applyColumnsState: function (columns) {
        if (!columns || !columns.length) {
            return;
        }

        var me = this,
            items = me.items.items,
            count = items.length,
            i = 0,
            length = columns.length,
            c, col, columnState, index;

        for (c = 0; c < length; c++) {
            columnState = columns[c];

            for (index = count; index--;) {
                col = items[index];
                if ((col.getStateId && columnState.id && col.getStateId() == columnState.id)
                    || (!columnState.id && col.dataIndex == columnState.dataIndex)) {
                    // If a column in the new grid matches up with a saved state...
                    // Ensure that the column is restored to the state order.
                    // i is incremented upon every column match, so all persistent
                    // columns are ordered before any new columns.
                    if (i !== index) {
                        me.moveHeader(index, i);
                    }

                    if (col.applyColumnState) {
                        col.applyColumnState(columnState);
                    }
                    ++i;
                    break;
                }
            }
        }
    }
});


Ext.override(Ext.dom.Helper, (function () {


    var afterbegin = 'afterbegin',
        afterend = 'afterend',
        beforebegin = 'beforebegin',
        beforeend = 'beforeend',
        ts = '<table>',
        te = '</table>',
        tbs = ts + '<tbody>',
        tbe = '</tbody>' + te,
        trs = tbs + '<tr>',
        tre = '</tr>' + tbe,
        detachedDiv = document.createElement('div'),
        bbValues = ['BeforeBegin', 'previousSibling'],
        aeValues = ['AfterEnd', 'nextSibling'],
        bb_ae_PositionHash = {
            beforebegin: bbValues,
            afterend: aeValues
        },
        fullPositionHash = {
            beforebegin: bbValues,
            afterend: aeValues,
            afterbegin: ['AfterBegin', 'firstChild'],
            beforeend: ['BeforeEnd', 'lastChild']
        };


    return {
        extend: Ext.dom.AbstractHelper,


        tableRe: /^(?:table|thead|tbody|tr|td)$/i,

        tableElRe: /td|tr|tbody|thead/i,


        useDom: false,


        createDom: function (o, parentNode) {
            var el,
                doc = document,
                useSet,
                attr,
                val,
                cn,
                i, l;

            if (Ext.isArray(o)) {
                el = doc.createDocumentFragment();
                for (i = 0, l = o.length; i < l; i++) {
                    this.createDom(o[i], el);
                }
            } else if (typeof o == 'string') {
                el = doc.createTextNode(o);
            } else {
                el = doc.createElement(o.tag || 'div');
                useSet = !!el.setAttribute;
                for (attr in o) {
                    if (!this.confRe.test(attr)) {
                        val = o[attr];
                        if (attr == 'cls') {
                            el.className = val;
                        } else {
                            if (useSet) {
                                el.setAttribute(attr, val);
                            } else {
                                el[attr] = val;
                            }
                        }
                    }
                }
                Ext.DomHelper.applyStyles(el, o.style);

                if ((cn = o.children || o.cn)) {
                    this.createDom(cn, el);
                } else if (o.html) {
                    el.innerHTML = o.html;
                }
            }
            if (parentNode) {
                parentNode.appendChild(el);
            }
            return el;
        },

        ieTable: function (depth, openingTags, htmlContent, closingTags) {
            detachedDiv.innerHTML = [openingTags, htmlContent, closingTags].join('');

            var i = -1,
                el = detachedDiv,
                ns;
            //Проверка el.firstChild на null для подвисающих при загрузке гридов
            while (++i < depth && el.firstChild != null) {
                el = el.firstChild;
            }

            ns = el.nextSibling;

            if (ns) {
                ns = el;
                el = document.createDocumentFragment();

                while (ns) {
                    nx = ns.nextSibling;
                    el.appendChild(ns);
                    ns = nx;
                }
            }
            return el;
        },


        insertIntoTable: function (tag, where, destinationEl, html) {
            var node,
                before,
                bb = where == beforebegin,
                ab = where == afterbegin,
                be = where == beforeend,
                ae = where == afterend;

            if (tag == 'td' && (ab || be) || !this.tableElRe.test(tag) && (bb || ae)) {
                return null;
            }
            before = bb ? destinationEl :
                     ae ? destinationEl.nextSibling :
                     ab ? destinationEl.firstChild : null;

            if (bb || ae) {
                destinationEl = destinationEl.parentNode;
            }

            if (tag == 'td' || (tag == 'tr' && (be || ab))) {
                node = this.ieTable(4, trs, html, tre);
            } else if (((tag == 'tbody' || tag == 'thead') && (be || ab)) ||
                    (tag == 'tr' && (bb || ae))) {
                node = this.ieTable(3, tbs, html, tbe);
            } else {
                node = this.ieTable(2, ts, html, te);
            }
            destinationEl.insertBefore(node, before);
            return node;
        },


        createContextualFragment: function (html) {
            var fragment = document.createDocumentFragment(),
                length, childNodes;

            detachedDiv.innerHTML = html;
            childNodes = detachedDiv.childNodes;
            length = childNodes.length;


            while (length--) {
                fragment.appendChild(childNodes[0]);
            }
            return fragment;
        },

        applyStyles: function (el, styles) {
            if (styles) {
                if (typeof styles == "function") {
                    styles = styles.call();
                }
                if (typeof styles == "string") {
                    styles = Ext.dom.Element.parseStyles(styles);
                }
                if (typeof styles == "object") {
                    Ext.fly(el, '_applyStyles').setStyle(styles);
                }
            }
        },


        createHtml: function (spec) {
            return this.markup(spec);
        },

        doInsert: function (el, o, returnElement, pos, sibling, append) {

            el = el.dom || Ext.getDom(el);

            var newNode;

            if (this.useDom) {
                newNode = this.createDom(o, null);

                if (append) {
                    el.appendChild(newNode);
                }
                else {
                    (sibling == 'firstChild' ? el : el.parentNode).insertBefore(newNode, el[sibling] || el);
                }

            } else {
                newNode = this.insertHtml(pos, el, this.markup(o));
            }
            return returnElement ? Ext.get(newNode, true) : newNode;
        },


        overwrite: function (el, html, returnElement) {
            var newNode;

            el = Ext.getDom(el);
            html = this.markup(html);


            if (Ext.isIE && this.tableRe.test(el.tagName)) {

                while (el.firstChild) {
                    el.removeChild(el.firstChild);
                }
                if (html) {
                    newNode = this.insertHtml('afterbegin', el, html);
                    return returnElement ? Ext.get(newNode) : newNode;
                }
                return null;
            }
            el.innerHTML = html;
            return returnElement ? Ext.get(el.firstChild) : el.firstChild;
        },

        insertHtml: function (where, el, html) {
            var hashVal,
                range,
                rangeEl,
                setStart,
                frag;

            where = where.toLowerCase();


            if (el.insertAdjacentHTML) {


                if (Ext.isIE && this.tableRe.test(el.tagName) && (frag = this.insertIntoTable(el.tagName.toLowerCase(), where, el, html))) {
                    return frag;
                }

                if ((hashVal = fullPositionHash[where])) {

                    if (Ext.global.MSApp && Ext.global.MSApp.execUnsafeLocalFunction) {

                        MSApp.execUnsafeLocalFunction(function () {
                            el.insertAdjacentHTML(hashVal[0], html);
                        });
                    } else {
                        el.insertAdjacentHTML(hashVal[0], html);
                    }

                    return el[hashVal[1]];
                }

            } else {

                if (el.nodeType === 3) {
                    where = where === 'afterbegin' ? 'beforebegin' : where;
                    where = where === 'beforeend' ? 'afterend' : where;
                }
                range = Ext.supports.CreateContextualFragment ? el.ownerDocument.createRange() : undefined;
                setStart = 'setStart' + (this.endRe.test(where) ? 'After' : 'Before');
                if (bb_ae_PositionHash[where]) {
                    if (range) {
                        range[setStart](el);
                        frag = range.createContextualFragment(html);
                    } else {
                        frag = this.createContextualFragment(html);
                    }
                    el.parentNode.insertBefore(frag, where == beforebegin ? el : el.nextSibling);
                    return el[(where == beforebegin ? 'previous' : 'next') + 'Sibling'];
                } else {
                    rangeEl = (where == afterbegin ? 'first' : 'last') + 'Child';
                    if (el.firstChild) {
                        if (range) {
                            range[setStart](el[rangeEl]);
                            frag = range.createContextualFragment(html);
                        } else {
                            frag = this.createContextualFragment(html);
                        }

                        if (where == afterbegin) {
                            el.insertBefore(frag, el.firstChild);
                        } else {
                            el.appendChild(frag);
                        }
                    } else {
                        el.innerHTML = html;
                    }
                    return el[rangeEl];
                }
            }
        },


        createTemplate: function (o) {
            var html = this.markup(o);
            return new Ext.Template(html);
        }

    };
})(), function () {
    Ext.ns('Ext.core');
    Ext.DomHelper = Ext.core.DomHelper = new this;
});

//Ext.override(Ext.draw.engine.Svg, {
//    applyZIndex: function (sprite) {
//        /* Bug fix:
//        * applyZIndex doesn't reorder the sprites in the items mixedCollection.
//        * This causes the sprites to be sorted incorrectly.
//        */
//        this.items.remove(sprite);
//        this.insertByZIndex(sprite);
//        this.callOverridden(arguments);
//    }
//});

//Ext.override(Ext.chart.Chart, {
//    chartMargins: new Ext.util.MixedCollection(),

//    setZoom: function (zoomConfig) {
//        var me = this,
//            axesItems = me.axes.items,
//            i, ln, axis,
//            bbox = me.chartBBox,
//            xScale = bbox.width,
//            yScale = bbox.height,
//            zoomArea = {
//                x: zoomConfig.x - me.el.getX(),
//                y: zoomConfig.y - me.el.getY(),
//                width: zoomConfig.width,
//                height: zoomConfig.height
//            },
//            zoomer, ends, from, to, store, count, step, length, horizontal;

//        for (i = 0, ln = axesItems.length; i < ln; i++) {
//            axis = axesItems[i];
//            horizontal = (axis.position == 'bottom' || axis.position == 'top');
//            if (axis.type == 'Category') {
//                if (!store) {
//                    store = me.getChartStore();
//                    count = store.data.items.length;
//                }
//                zoomer = zoomArea;
//                length = axis.length;
//                step = Math.round(length / count);
//                if (horizontal) {
//                    from = (zoomer.x ? Math.floor(zoomer.x / step) + 1 : 0);
//                    to = (zoomer.x + zoomer.width) / step;
//                } else {
//                    from = (zoomer.y ? Math.floor(zoomer.y / step) + 1 : 0);
//                    to = (zoomer.y + zoomer.height) / step;
//                }
//            }
//            else {
//                zoomer = {
//                    x: zoomArea.x / xScale,
//                    y: zoomArea.y / yScale,
//                    width: zoomArea.width / xScale,
//                    height: zoomArea.height / yScale
//                }
//                ends = axis.calcEnds();
//                if (horizontal) {
//                    from = (ends.to - ends.from) * zoomer.x + ends.from;
//                    to = (ends.to - ends.from) * zoomer.width + from;
//                } else {
//                    to = (ends.to - ends.from) * (1 - zoomer.y) + ends.from;
//                    from = to - (ends.to - ends.from) * zoomer.height;
//                }
//            }
//            axis.minimum = from;
//            axis.maximum = to;
//        }
//        me.redraw(false);
//    },

//    redraw: function (resize) {
//        this.callParent(arguments);
//        this.drawChartMargins();
//    },

//    drawChartMargins: function () {
//        var me = this,
//            fill = me.surface.background ? me.surface.background.fill : 'white',
//            sprite;

//        sprite = me.chartMargins.get('left');
//        if (!sprite) {
//            sprite = Ext.create('Ext.draw.Sprite', {
//                type: 'rect',
//                x: 0,
//                y: 0,
//                zIndex: 5000,
//                surface: me.surface
//            });
//            me.chartMargins.add('left', sprite);
//        }
//        sprite.setAttributes({
//            fill: fill,
//            width: me.chartBBox.x,
//            height: me.chartBBox.height + me.chartBBox.y
//        });

//        sprite = me.chartMargins.get('bottom');
//        if (!sprite) {
//            sprite = Ext.create('Ext.draw.Sprite', {
//                type: 'rect',
//                x: 0,
//                zIndex: 5000,
//                surface: me.surface
//            });
//            me.chartMargins.add('bottom', sprite);
//        }
//        sprite.setAttributes({
//            fill: fill,
//            width: me.curWidth,
//            height: me.curHeight - me.chartBBox.height - me.chartBBox.y,
//            y: me.chartBBox.height + me.chartBBox.y
//        });

//        sprite = me.chartMargins.get('rigth');
//        if (!sprite) {
//            sprite = Ext.create('Ext.draw.Sprite', {
//                type: 'rect',
//                y: 0,
//                zIndex: 5000,
//                surface: me.surface
//            });
//            me.chartMargins.add('rigth', sprite);
//        }
//        sprite.setAttributes({
//            fill: fill,
//            width: me.curWidth - me.chartBBox.x - me.chartBBox.width,
//            height: me.chartBBox.height + me.chartBBox.y,
//            x: me.chartBBox.x + me.chartBBox.width
//        });

//        me.chartMargins.each(function (sprite) {
//            me.surface.renderItem(sprite);
//        });
//    }
//});

//Ext.override(Ext.chart.axis.Axis, {
//    getOrCreateLabel: function (i, text) {
//        var label = this.callParent(arguments);
//        label.setAttributes({
//            zIndex: 5001
//        });
//        return label;
//    },

//    drawTitle: function (maxWidth, maxHeight) {
//        this.callParent(arguments);
//        this.displaySprite.setAttributes({
//            zIndex: 5001
//        });
//    },

//    drawAxis: function (init) {
//        this.callParent(arguments);
//        this.axis.setAttributes({
//            zIndex: 5001
//        });
//    }
//});

//Ext.override(Ext.chart.axis.Numeric, {
//    doConstrain: function () {
//        var me = this,
//            store = me.chart.store,
//            items = store.data.items,
//            d, dLen, record,
//            series = me.chart.series.items,
//            fields = me.fields,
//            ln = fields.length,
//            range = me.calcEnds(),
//            min = range.from, max = range.to, i, l,
//            useAcum = false,
//            value, data = [],
//            addRecord;

//        for (i = 0, l = series.length; i < l; i++) {
//            if (series[i].type === 'bar' && series[i].stacked) {
//                // Do not constrain stacked bar chart.
//                return;
//            }
//        }

//        for (d = 0, dLen = items.length; d < dLen; d++) {
//            addRecord = true;
//            record = items[d];
//            for (i = 0; i < ln; i++) {
//                value = record.get(fields[i]);
//                if (+value < +min) {
//                    addRecord = false;
//                    break;
//                }
//                if (+value > +max) {
//                    addRecord = false;
//                    break;
//                }
//            }
//            if (addRecord) {
//                data.push(record);
//            }
//        }
//        me.chart.substore = Ext.create('Ext.data.Store', { model: store.model });
//        me.chart.substore.loadData(data); // data records must be loaded (not passed as config above because it's not json)
//    }
//});