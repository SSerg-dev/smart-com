Ext.define('Ext.ux.grid.plugin.CopyPaste', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.copypaste',

    selectedCellCls: 'cell-copy-paste-selection',

    // Выделенная область.
    startPoint: null,
    endPoint: null,

    // Очередь выделенных областей.
    // Предыдущая выделенная область.
    prevStartPoint: null,
    prevEndPoint: null,

    // Следующая выделенная область.
    nextStartPoint: null,
    nextEndPoint: null,

    constructor: function () {
        this.callParent(arguments);
    },

    init: function (grid) {
        var view = this.view = grid.getView();

        grid.on({
            scope: this,
            beforeedit: this.onBeforeEdit
        });

        if (view.isLockingView) {
            this.bindView(view.lockedView);
            this.bindView(view.normalView);
        } else {
            this.bindView(view);
        }

        this.addKeyMap(grid);
        this.createTextBuferArea(grid);
    },

    destroy: function () {
        this.keyMap && this.keyMap.destroy();
        this.texBufer && this.textBufer.destroy();
    },

    bindView: function (view) {

        if (!view.rendered) {
            view.on({
                scope: this,
                single: true,
                afterrender: Ext.Function.bind(this.bindView, this, [view], 0)
            });
            return;
        }

        view.on({
            scope: this,
            cellmousedown: this.onCellMouseDown
        });

        view.mon(view.getTargetEl(), {
            scope: this,
            mouseover: Ext.Function.bind(this.handleViewMouseOver, this, [view], 0)
        });
    },

    addKeyMap: function (grid) {
        if (!grid.rendered) {
            grid.on({
                scope: this,
                single: true,
                afterrender: Ext.Function.bind(this.addKeyMap, this, [grid], 0)
            });
            return;
        }

        this.keyMap = new Ext.util.KeyMap({
            target: grid.getTargetEl(),
            binding: [{
                key: 'c',
                scope: this,
                ctrl: true,
                handler: this.onCtrlCPressed
            }]
        });
    },


    // Обработчики событий.
    // ================================================================================

    // TODO: Исправить фильтр переходов, чтобы он реагировал на переходы только между ячейками.
    handleViewMouseOver: function (view, e) {
        var item = e.getTarget(view.getItemSelector(), view.getTargetEl()),
            cell = e.getTarget(view.getCellSelector(), item),
            itemIndex = -1,
            cellIndex = -1,
            record,
            header;

        // Игнорируем перемещение курсора с/на дочерние элементы ячейки.
        if (e.within(e.getRelatedTarget()) || e.within(e.getTarget(), true)) {
            return;
        }

        if (item) {
            record = view.getRecord(item);
            itemIndex = view.indexInStore ? view.indexInStore(record) : view.indexOf(item);
        }

        // cellIndex is an attribute only of TD elements. Other TplFactories must use the data-cellIndex attribute.
        if (cell) {
            if (!cell.parentNode) {
                // If we have no parentNode, the td has been removed from the DOM, probably via an update,
                // so just jump out since the target for the event isn't valid
                return;
            }

            // We can't use the cellIndex because we don't render hidden columns
            header = view.getHeaderByCell(cell);
            // Find the index of the header in the *full* (including hidden columns) leaf column set.
            // Because In 4.0.0 we rendered hidden cells, and the cellIndex included the hidden ones.
            cellIndex = view.ownerCt.getColumnManager().getHeaderIndex(header);
        }

        this.onCellMouseEnter(view, cell, cellIndex, record, item, itemIndex, e);
    },

    onBeforeEdit: function () {
        console.log(arguments);
        this.start(null);
        this.change(null);
        this.commit();
    },

    onCellMouseDown: function (view, cell, cellIndex, record, row, recordIndex, e) {
        var sm = this.cmp.getSelectionModel();

        if (cellIndex === -1 || recordIndex === -1) {
            return;
        }

        if (e.button === 0) {
            if (e.shiftKey === false) {
                // Start position.
                this.start({
                    view: view,
                    row: row,
                    column: cellIndex
                });
            } else {

                // End position.
                this.start();
                this.change({
                    view: view,
                    row: row,
                    column: cellIndex
                });
            }

            this.commit();
        } else {
            // TODO: Убрать этот код отсюда.
            if (view.ownerCt.isLocked) {
                this.start({
                    view: this.view.lockedView,
                    row: row,
                    column: 0
                }, true);
                this.change({
                    view: this.view.normalView,
                    row: row,
                    column: this.view.normalView.getHeaderCt().getGridColumns().length - 1
                });
                this.commit();
            }
        }

        if (this.isArea()) {
            sm.deselectAll();
        }

        sm.setLocked(this.isArea());
    },

    onCellMouseEnter: function (view, cell, cellIndex, record, row, recordIndex, e) {
        var sm = this.cmp.getSelectionModel();

        if (cellIndex === -1 || recordIndex === -1) {
            return;
        }

        if (Ext.isIE ? e.browserEvent.buttons === 1 : e.button === 0) {
            this.start();
            this.change({
                view: view,
                row: row,
                column: cellIndex
            });
            this.commit();

            if (this.isArea()) {
                sm.deselectAll();
            }

            sm.setLocked(this.isArea());
        }
    },

    onCtrlCPressed: function () {
        var tb = this.textBufer,
            data = this.collectAreaData(this.getArea());

        if (!Ext.isEmpty(data)) {
            tb.dom.value = data;
            tb.focus();
            tb.dom.select();// setSelectionRange(0, tb.dom.value.length);
        }
    },


    // Внутренний интерфейс для работы с выделенной областью.
    // ================================================================================

    // TODO: Отладить для полного сброса выделения.
    start: function (pos, updateOnChange) {
        this.load();
        this.push();

        if (Ext.isDefined(pos)) {
            this.create(pos);

            if (!updateOnChange) {
                this.updateArea();
                this.push();
            }
        }
    },

    change: function (pos) {
        if (pos && !this.nextStartPoint) {
            this.start(pos, true);
        }

        this.resize(pos);
        this.updateArea();
        this.push();
    },

    commit: function () {
        if (this.nextStartPoint) {
            this.save();
            this.clear();
        }
    },

    /*
    rollback: function () {
        if (this.nextStartPoint) {
            this.push();
            this.load();
            this.updateArea();
            this.clear();
        }
    },
    */

    isArea: function () {
        var sp = this.nextStartPoint || this.startPoint,
            ep = (this.nextStartPoint ? this.nextEndPoint : this.endPoint);

        return sp && ep && (sp.row !== ep.row || sp.view === ep.view && sp.column !== ep.column || sp.view !== ep.view);
    },

    // Методы для работы с очередью выделенных областей.
    // ================================================================================

    load: function () {
        this.nextStartPoint = this.startPoint;
        this.nextEndPoint = this.endPoint;
    },

    save: function () {
        this.startPoint = this.prevStartPoint;
        this.endPoint = this.prevEndPoint;
    },

    push: function () {
        this.prevStartPoint = this.nextStartPoint;
        this.prevEndPoint = this.nextEndPoint;
    },

    create: function (pos) {
        if (pos) {
            this.nextStartPoint = new this.SelectionPoint(this);
            this.nextStartPoint.setPosition(pos);
        } else {
            this.nextStartPoint = null;
        }

        this.nextEndPoint = null;
    },

    resize: function (pos) {
        if (pos) {
            this.nextEndPoint = new this.SelectionPoint(this);
            this.nextEndPoint.setPosition(pos);
        } else {
            this.nextEndPoint = null;
        }
    },

    clear: function () {
        this.prevStartPoint = this.nextStartPoint = null;
        this.prevEndPoint = this.nextEndPoint = null;
    },


    // Методы для для отрисовки выделенной области.
    // ================================================================================

    /*
    * Выполняет перерисовку выделенной области.
    */
    updateArea: function () {
        var me = this,
            cAreas = this.splitAreaByView(this.getPrevArea()),
            nAreas = this.splitAreaByView(this.getNextArea()),
            ordered = this.orderAreasByView(cAreas, nAreas),
            areas;

        areas = ordered.map(function (pair) {
            if (pair.curr && pair.next) {
                return this.mergeAreas(pair.curr, pair.next);
            } else if (pair.next && !pair.curr) {
                return {
                    remove: [],
                    add: [pair.next]
                }
            } else if (pair.curr && !pair.next) {
                return {
                    remove: [pair.curr],
                    add: []
                }
            }
        }, this).reduce(function (prev, curr) {
            if (prev) {
                prev.remove = prev.remove.concat(curr.remove);
                prev.add = prev.add.concat(curr.add);
            }
            return prev || curr;
        }, null);

        if (areas) {
            areas.remove.forEach(function (area) {
                this.enumerateCells(area, function (view, row, column) {
                    var cell = view.getCellByPosition({ row: row, column: column });
                    if (cell) {
                        cell.removeCls(me.selectedCellCls);
                    }
                });
            }, this);
            areas.add.forEach(function (area) {
                this.enumerateCells(area, function (view, row, column) {
                    var cell = view.getCellByPosition({ row: row, column: column });
                    if (cell) {
                        cell.addCls(me.selectedCellCls);
                    }
                });
            }, this);
        }

        return areas;
    },

    /*
    * Возвращает текущую выделенную область.
    */
    getPrevArea: function () {
        var sp = this.prevStartPoint,
            ep = this.prevEndPoint;

        return sp ? {
            sp: sp,
            ep: ep || sp
        } : null;
    },

    /*
    * Возвращает измененную выделенную область.
    */
    getNextArea: function () {
        var sp = this.nextStartPoint,
            ep = this.nextEndPoint;

        return sp ? {
            sp: sp,
            ep: ep || sp
        } : null;
    },

    /*
    * Выполняет функцию callback для каждой ячейки в выделенной области area.
    */
    enumerateCells: function (area, callback) {
        this.splitAreaByView(area).forEach(function (area) {
            var view = area.sp.view,
                yOffset = area.sp.row,
                xOffset = area.sp.column,
                h = Math.abs(area.ep.row - yOffset),
                w = Math.abs(area.ep.column - xOffset),
                yDir = this.sign(area.ep.row - yOffset),
                xDir = this.sign(area.ep.column - xOffset),
                row,
                column;

            for (var y = 0; y <= h; y++) {
                row = yDir * y + yOffset;
                for (var x = 0; x <= w; x++) {
                    column = xDir * x + xOffset;
                    callback.call(this, view, row, column);
                }
            }
        }, this);
    },

    /*
    * Если выделенная область area расположена на нескольких представлениях, то
    * выполняется разбиение выделенной области на части, каждая из которых 
    * находится на отдельном представлении. 
    */
    splitAreaByView: function (area) {
        var leftPoint,
            rightPoint;

        if (!area) {
            return [];
        }

        if (area.sp.view !== area.ep.view) {
            if (area.ep.view.getX() < area.sp.view.getX()) {
                // ep | sp
                leftPoint = area.ep;
                rightPoint = area.sp;
            } else {
                // sp | ep
                leftPoint = area.sp;
                rightPoint = area.ep;
            }

            return [{
                sp: leftPoint,
                ep: {
                    view: leftPoint.view,
                    row: rightPoint.row,
                    column: leftPoint.view.getHeaderCt().getGridColumns().length - 1
                }
            }, {
                sp: {
                    view: rightPoint.view,
                    row: leftPoint.row,
                    column: 0
                },
                ep: rightPoint
            }];
        }

        return [area];
    },

    /*
    * Сопоставляет части текущей выделенной области 
    * частям измененной выделенной области. 
    * Ключом является представление.
    */
    orderAreasByView: function (current, next) {
        var nextClone = next.slice(0);

        return current.map(function (cArea) {
            var item = { curr: cArea, next: null };
            for (var i = 0; i < nextClone.length; i++) {
                if (cArea.sp.view === nextClone[i].sp.view) {
                    item.next = nextClone[i];
                    nextClone = nextClone.slice(0, i).concat(nextClone.slice(i + 1));
                    break;
                }
            }
            return item;
        }).concat(nextClone.map(function (nArea) {
            return {
                curr: null,
                next: nArea
            };
        }));
    },

    /*
    * Выполняет слияние текущей и новой выделенной области.
    * Возвращает списки областей, которые необходимо добавить 
    * или удалить из текущей, чтобы получить новую выделенную область.
    */
    mergeAreas: function (current, next) {
        var xChanges = this.mergeSegments(current.sp.column, current.ep.column, next.sp.column, next.ep.column),
            yChanges = this.mergeSegments(current.sp.row, current.ep.row, next.sp.row, next.ep.row),
            rQueue = [],
            aQueue = [];

        yChanges.forEach(function (yChange) {
            xChanges.forEach(function (xChange) {
                var op = yChange.op + xChange.op,
                    queue = op > 0 ? aQueue : op < 0 ? rQueue : null;

                if (queue) {
                    queue.push({
                        sp: { view: current.sp.view, row: yChange.low, column: xChange.low },
                        ep: { view: current.sp.view, row: yChange.high, column: xChange.high }
                    });
                }
            }, this);
        }, this);

        return {
            remove: rQueue,
            add: aQueue
        };
    },


    // Вспомогательные методы.
    // ================================================================================

    /*
    * Выполняет слияние двух отрезков.
    * Возвращает изменения, которые произошли с текущим отрезком 
    * для получения нового отрезка.
    */
    mergeSegments: function (segLow, segHigh, nextSegLow, nextSegHigh) {
        var swap,
            res = [];

        if (segLow > segHigh) {
            swap = segLow;
            segLow = segHigh;
            segHigh = swap;
        }

        if (nextSegLow > nextSegHigh) {
            swap = nextSegLow;
            nextSegLow = nextSegHigh;
            nextSegHigh = swap;
        }

        // Отрезки перекрываются.
        if (segLow <= nextSegHigh && nextSegLow <= segHigh) {
            // Неизменившаяся часть отрезка.
            res.push({
                op: 0,
                // Max (segLow, nextSegLow).
                low: (segLow > nextSegLow) ? segLow : nextSegLow,
                // Min (segHigh, nextSegHigh).
                high: (segHigh < nextSegHigh) ? segHigh : nextSegHigh
            });

            // Нижняя граница отрезка сместилась влево.
            // Отрезок увеличился.
            if (nextSegLow < segLow) {
                res.push({
                    op: 1,
                    low: nextSegLow,
                    high: segLow - 1
                });
            } else if (segLow < nextSegLow) {
                // Нижняя граница отрезка сместилась вправо.
                // Отрезок уменьшился.
                res.push({
                    op: -1,
                    low: segLow,
                    high: nextSegLow - 1
                });
            }

            // Верхняя граница отрезка сместилась вправо.
            // Отрезок увеличился.
            if (nextSegHigh > segHigh) {
                res.push({
                    op: 1,
                    low: segHigh + 1,
                    high: nextSegHigh
                });
            } else if (segHigh > nextSegHigh) {
                // Верхняя граница отрезка сместилась влево.
                // Отрезок уменьшился.
                res.push({
                    op: -1,
                    low: nextSegHigh + 1,
                    high: segHigh
                });
            }
        } else {
            // Отрезки не перекрываются.
            res.push({
                op: -1,
                low: segLow,
                high: segHigh
            });
            res.push({
                op: 1,
                low: nextSegLow,
                high: nextSegHigh
            });
        }

        return res;
    },

    sign: function (x) {
        return (x === 0 || isNaN(x))
            ? x
            : (x > 0) ? 1 : -1
    },


    collectAreaData: function (area) {
        var columns = this.getColumns(area);

        return this.getRecords(area).map(function (rec) {
            return columns.map(function (col) {
                return rec.get(col.dataIndex);
            }).join('\t');
        }).join('\n');
    },

    getRecords: function (area) {
        if (area) {
            var sIdx = area.sp.row,
                eIdx = area.ep.row;

            return this.view.getStore()
                .getRange(Math.min(sIdx, eIdx), Math.max(sIdx, eIdx));
        }

        return [];
    },

    getColumns: function (area) {
        if (area) {
            return this.splitAreaByView(area).reduce(function (columns, area) {
                var sIdx = area.sp.column,
                    eIdx = area.ep.column,
                    columnsPart = area.sp.view.headerCt.getGridColumns()
                        .slice(Math.min(sIdx, eIdx), Math.max(sIdx, eIdx) + 1);

                return columns.concat(columnsPart);
            }, []).filter(function (column) {
                return !column.isHidden();
            });
        }

        return [];
    },

    getArea: function () {
        var sp = this.startPoint,
            ep = this.endPoint;

        return sp ? {
            sp: sp,
            ep: ep || sp
        } : null;
    },

    createTextBuferArea: function (grid) {
        if (!grid.rendered) {
            grid.on({
                scope: this,
                single: true,
                afterrender: Ext.Function.bind(this.createTextBuferArea, this, [grid], 0)
            });
            return;
        }

        var tb = this.textBufer = grid.getEl().appendChild({
            id: 'copy-paste-bufer',
            tag: 'textarea'
        });

        tb.applyStyles({
            'z-index': -1,
            position: 'absolute',
            top: '-0px',
            left: '-1000px'
        });

        tb.addListener('keyup', function (e) {
            grid.focus();
        }, this);
    }

}, function () {

    // Encapsulate a single selection position.
    // Maintains { row: n, column: n, record: r, columnHeader: c}
    var SelectionPoint = this.prototype.SelectionPoint = function (model) {
        this.model = model;
        this.view = model.primaryView;
    };

    // Selection row/record & column/columnHeader
    SelectionPoint.prototype.setPosition = function (row, col) {
        var me = this;

        // We were passed {row: 1, column: 2, view: myView}
        if (arguments.length === 1) {

            // SelectionModel is shared between both sides of a locking grid.
            // It can be positioned on either view.
            if (row.view) {
                me.view = row.view;
            }
            col = row.column;
            row = row.row;
        }

        me.setRow(row);
        me.setColumn(col);
        return me;
    };

    SelectionPoint.prototype.setRow = function (row) {
        var me = this;
        if (row !== undefined) {
            // Row index passed
            if (typeof row === 'number') {
                me.row = Math.max(Math.min(row, me.view.store.getCount() - 1), 0);
                me.record = me.view.store.getAt(row);
            }
                // row is a Record
            else if (row.isModel) {
                me.record = row;
                me.row = me.view.indexOf(row);
            }
                // row is a grid row
            else if (row.tagName) {
                me.record = me.view.getRecord(row);
                me.row = me.view.indexOf(me.record);
            }
        }
    };

    SelectionPoint.prototype.setColumn = function (col) {
        var me = this;
        if (col !== undefined) {
            // column index passed
            if (typeof col === 'number') {
                me.column = col;
                me.columnHeader = me.view.getHeaderAtIndex(col);
            }
                // column Header passed
            else if (col.isHeader) {
                me.columnHeader = col;
                me.column = col.getIndex();
            }
        }
    };

});
