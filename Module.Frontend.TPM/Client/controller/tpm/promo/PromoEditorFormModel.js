Ext.define('App.controller.tpm.promo.PromoEditorFormModel', {
    extend: 'Core.form.EditorFormModel',

    onOkButtonClick: function (button) {
        var me = this,
        form = this.getForm(),
        grid = this.getGrid(),
        confirmFn = form.commitChanges(),
        record = form.getRecord();
        if (!form.isValid()) {
            confirmFn(false);
            return;
        };
        var picker = form.down('[name=ColorId]');
        var filter = form.down('[name=ProductFilter]').getValue();
        var parameters = {
            $actionName: 'GetSuitable',
            $method: 'POST',
            productFilter: filter
        };
        form.setLoading(true);
        breeze.EntityQuery
            .from('Colors')
            .withParameters(parameters)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                form.setLoading(false);
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    if (result.data.length == 1) {
                        form.form.updateRecord();
                        record.set('ColorId', result.data[0]);
                        if (record.afterUpdate) {
                            record.afterUpdate(grid);
                        }
                        me.saveModel(record, confirmFn);
                    } else {
                        picker.afterPickerCreate = function (scope) {
                            scope.getStore().setFixedFilter('ColorFilter', {
                                property: 'Id',
                                operation: 'In',
                                value: result.data
                            });
                        }
                        picker.onSelectButtonClick = function (button) {
                            var picker = this.picker,
                                selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                                selrecord = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                            this.setValue(selrecord);
                            if (this.needUpdateMappings) {
                                this.updateMappingValues(selrecord);
                            }
                            this.afterSetValue(selrecord);
                            picker.close();
                            form.form.updateRecord();
                            if (record.afterUpdate) {
                                record.afterUpdate(grid);
                            }
                            me.saveModel(record, confirmFn);
                        };
                        var win = picker.createPicker();

                        if (win) {
                            win.show();
                            picker.getStore().load();
                        }
                        App.Notify.pushInfo(l10n.ns('tpm', 'message').value('ColorChoose'));
                    }
                } else {
                    App.Notify.pushError(result.message);
                }
            })
            .fail(function (data) {
                form.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },

    addHandlers: function (toolbar) {
        var isCreate = toolbar.itemId == 'createtoolbar';
        firstOpen = isCreate ? this.firstCreateOpen : this.firstEditOpen;
        if (firstOpen && toolbar) {
            toolbar.down('#ok').on({
                scope: this,
                click: this.onOkButtonClick
            });

            toolbar.down('#cancel').on({
                scope: this,
                click: this.onCancelButtonClick
            });
            isCreate ? this.firstCreateOpen = false : this.firstEditOpen = false;
        }
    },
    //saveModel: function (model, callback) {
    //    var isCreate = model.phantom,
    //        grid = this.getGrid(),
    //        panel = grid.up('combineddirectorypanel');

    //    panel.setLoading(l10n.ns('core').value('savingText'));

    //    model.save({
    //        scope: this,
    //        success: function () {
    //            if (callback) {
    //                callback(true);
    //            }

    //            grid.setLoadMaskDisabled(true);

    //            grid.getStore().on({
    //                single: true,
    //                scope: this,
    //                load: function (records, operation, success) {
    //                    model.set('Key');
    //                    var activeView = panel.getLayout().getActiveItem();

    //                    if (activeView.getItemId() === 'detailform') {
    //                        var formRecord = activeView.getRecord();

    //                        if (formRecord && formRecord.getId() === model.getId()) {
    //                            if (formRecord.afterUpdate) {
    //                                formRecord.afterUpdate(grid);
    //                            }

    //                            formRecord.set('Key');
    //                            activeView.loadRecord(model);
    //                            if (isCreate) {
    //                                grid.getSelectionModel().select(model);
    //                            }
    //                        }
    //                    }

    //                    panel.setLoading(false);
    //                    grid.setLoadMaskDisabled(false);

    //                    if (typeof grid.afterSaveCallback === 'function') {
    //                        grid.afterSaveCallback(grid);
    //                    }

    //                    this.switchToLastActiveItem();
    //                }
    //            });

    //            grid.getStore().load();
    //        },
    //        failure: function () {
    //            if (callback) {
    //                callback(false);
    //            }
    //            model.reject();
    //            panel.setLoading(false);
    //        }
    //    });
    //},
});