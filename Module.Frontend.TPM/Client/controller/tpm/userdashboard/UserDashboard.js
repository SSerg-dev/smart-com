Ext.define('App.controller.tpm.userdashboard.UserDashboard', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'userdashboard #panel1': {
                    afterrender: this.onRender
                },
                'userdashboard button[itemId!=ok]': {
                    click: this.onButtonClick2
                },



            }
        });
    },

    onRender: function (window) {       
        this.getCard(window);
    },

    onButtonClick2: function (window) { 
        var panel = window.up().up();
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var promoController = App.app.getController('core.Main');
        var vc = promoController.getViewContainer(),
            view = vc.getComponent(panel.widget);

        if (!view) {
            view = Ext.widget(panel.widget);

            if (view.isXType('associateddirectoryview')) {
                vc.addCls('associated-directory');
            } else {
                vc.removeCls('associated-directory');
            }

            Ext.suspendLayouts();
            vc.removeAll();
            vc.add(view);
            Ext.resumeLayouts(true);

            var grid = view.down('directorygrid'); 
            var store = grid.getStore();
            var extendedFilter = store.getExtendedFilter();
            extendedFilter.filter = panel.filter;
            store.filters.items = extendedFilter.filter;
            store.extendedFilter = extendedFilter;
                
            extendedFilter.needShowTextFilterFirst = true;

            vc.doLayout();
        }
    },
        getTimeCriticalKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.HOUR, 48);
            var filter = {
                operator: "or",
                rules: [{
                    operator: "and",
                    rules: [{
                        property: "PromoStatusName", operation: "Equals", value: 'Draft(published)'
                    },
                    {
                        property: "StartDate", operation: "LessThan", value: date
                    }
                    ]
                },
                {
                    operator: "and",
                    rules: [{
                        property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                    },
                    {
                        property: "DispatchesStart", operation: "LessThan", value: date
                    }
                    ]
                },
                {
                    operator: "and",
                    rules: [{
                        property: "PromoStatusName", operation: "Equals", value: 'Approved'
                    },
                    {
                        property: "StartDate", operation: "LessThan", value: date
                    }
                    ]
                }]
            }
            var widget = 'promo';
            var text = "Time Critical";
            var panel = 'panel1';
            var style = {
                styleStandart: 'panel-time-critical-standart',
                styleZero: 'panel-time-critical-zero'
            }
            var image = 'time_criticalnew.png';
            var color = '#e1312c';
            var buttonColor = '#feeded';
            return { filter: filter, widget: widget, text: text, panel: panel, style: style, image: image, color: color, buttonColor: buttonColor };
        },
        //to apporval
        getToApporvalKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, 61);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Draft(published)'
                }, {

                        property: "DispatchesStart", operation: "LessThan", value: date
                }]
            }
            var widget = 'promo';
            var text = "To approval";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#00009b';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //to plan
        getToPlanKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date = Ext.Date.add(date, Ext.Date.DAY, 4 * 7);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Approved'
                }, {
                        property: "StartDate", operation: "LessOrEqual", value: date
                    }]
            }

            var widget = 'promo';
            var text = "To Plan";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#00009b';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },

        //Upload Actuals
        getUploadActualsKeyAccountManager: function () {

            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Finished'
                }, {
                    operator: "or",
                    rules: [{
                        property: "ActualPromoLSVByCompensation", operation: "Equals", value: null
                    },
                    {
                        property: "ActualPromoLSVByCompensation", operation: "Equals", value: 0
                    }]
                }]
            }
            var widget = 'promo';
            var text = "Upload Actuals";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Promo to close
        getPromoToCloseKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date = Ext.Date.add(date, Ext.Date.DAY, -8 * 7);
            var filter = {
                operator: "and",
                rules: [
                    {
                        property: "PromoStatusName", operation: "Equals", value: 'Finished'
                    },
                    {
                        property: "EndDate", operation: "LessThan", value: date
                    },
                    {
                        operator: "or",
                        rules: [{
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: false
                                }, {
                                    property: "ActualPromoUpliftPercent", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoUpliftPercent", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoBaselineLSV", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoBaselineLSV", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoLSV", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoLSV", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: null
                                }
                            ]
                        }, {
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: true
                                }, {
                                    property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoIncrementalLSV", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoLSV", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoLSV", operation: "NotEqual", value: null
                                }, {
                                    property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: 0
                                }, {
                                    property: "ActualPromoLSVByCompensation", operation: "NotEqual", value: null
                                }
                            ]
                        }],
                    },
                ]
            };

            var widget = 'promo';
            var text = "Promo to Close";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Promo TI cost
        getPromoTICostKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);

            var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
            var filter = {
                operator: "and",
                rules: [
                    {
                        operator: "or",
                        rules: [{
                            property: "ActualCostTE", operation: "Equals", value: 0
                        }, {
                            property: "ActualCostTE", operation: "Equals", value: null
                        }],
                    }, {
                        property: "EndDate", operation: "LessOrEqual", value: dateEnd
                    }

                ]
            }
            var widget = 'associatedpromosupport';
            var text = "Promo TI cost";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Non-promo TI cost
        getNonPromoTICostKeyAccountManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);

            var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
            var filter = {
                operator: "and",
                rules: [
                    {
                        operator: "or",
                        rules: [{
                            property: "ActualCostTE", operation: "Equals", value: 0
                        }, {
                            property: "ActualCostTE", operation: "Equals", value: null
                        }],
                    }, {
                        property: "EndDate", operation: "LessOrEqual", value: dateEnd
                    }

                ]
            }
            var widget = 'associatednonpromosupport';
            var text = "Non-promo TI cost";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },

        //--DemandPlanning--
        //time critical
        getTimeCriticalDemandPlanning: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, 7);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                    },{
                        property: "IsCMManagerApproved", operation: "Equals", value: true
                    }, {
                        operator: "or",
                        rules: [{
                            property: "IsDemandPlanningApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: false
                        }]
                    }, 
                ]
            };

            var widget = 'promo';
            var text = "Time Critical";
            var panel = 'panel1';
            var style = {
                styleStandart: 'panel-time-critical-standart',
                styleZero: 'panel-time-critical-zero'
            }
            var image = 'time_criticalnew.png';
            var color = '#e1312c';
            var buttonColor = '#feeded';
            return { filter: filter, widget: widget, text: text, panel: panel, style: style, image: image, color: color, buttonColor: buttonColor };
        },
        //on apporval
        getOnApporvalDemandPlanning: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, 8 * 7);

            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                    }, {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                    },{
                        operator: "or",
                        rules: [{
                            property: "IsDemandPlanningApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandPlanningApproved", operation: "Equals", value: false
                        }]
                    }, 
                ]
            };
            var widget = 'promo';
            var text = "On approval";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#00009b';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Adjust data
        getAdjustDataDemandPlanning: function () {
   

            var filter = {
                operator: "and",
                rules: [
                    {
                        property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                    },
                    {
                        operator: "or",
                        rules: [{
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: false
                                },
                                {
                                    operator: "or",
                                    rules: [
                                        {
                                            property: "PlanPromoLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoLSV", operation: "Equals", value: null
                                        }, {
                                            property: "PlanPromoBaselineLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoBaselineLSV", operation: "Equals", value: null
                                        }, {
                                            property: "PlanPromoUpliftPercent", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoUpliftPercent", operation: "Equals", value: null
                                        }
                                    ]
                                }
                            ]
                        }, {
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: true
                                },
                                {
                                    operator: "or",
                                    rules: [
                                        {
                                            property: "PlanPromoLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoLSV", operation: "Equals", value: null
                                        }
                                    ]
                                }
                            ]
                        }],
                    },

                ]
            };
            var widget = 'promo';
            var text = "Adjust Data";
            var panel = 'panel2';
            var image = 'adjust_data.png';
            var color = '#0e0d9e';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Upload Actuals
        getUploadActualsDemandPlanning: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

             //При фильтрации Apload Actuals (DP) и Demand Planning Fact (DF) не должна учитываться крайняя дата, по этому -50
            date = Ext.Date.add(date, Ext.Date.DAY, -50);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Finished'
                }, {
                    operator: "or",
                    rules: [{
                        property: "ActualPromoLSV", operation: "Equals", value: null
                    },
                    {
                        property: "ActualPromoLSV", operation: "Equals", value: 0
                    }]
                },
                {

                    property: "EndDate", operation: "LessThan", value: date
                }
                ]
            }
            var widget = 'promo';
            var text = "Upload Actuals";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //--DemandFinance--
        getTimeCriticalDemandFinance: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.HOUR, 48);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                    }, {
                        property: "IsCMManagerApproved", operation: "Equals", value: true
                    }, {
                        property: "IsDemandPlanningApproved", operation: "Equals", value: true
                    },
                    {
                        operator: "or",
                        rules: [{
                            property: "IsDemandFinanceApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: false
                        }]
                    }, 
                ]
            };

            var widget = 'promo';
            var text = "Time Critical";
            var panel = 'panel1';
            var style = {
                styleStandart: 'panel-time-critical-standart',
                styleZero: 'panel-time-critical-zero'
            }
            var image = 'time_criticalnew.png';
            var color = '#e1312c';
            var buttonColor = '#feeded';
            return { filter: filter, widget: widget, text: text, panel: panel, style: style, image: image, color: color, buttonColor: buttonColor };
        },
        //Needs my approval
        getNeedsMyApprovalDemandFinance: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, 8 * 7);

            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                }, {
                    property: "IsCMManagerApproved", operation: "Equals", value: true
                }, {
                    property: "IsDemandPlanningApproved", operation: "Equals", value: true
                },  
                {
                        operator: "or",
                        rules: [{
                            property: "IsDemandFinanceApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsDemandFinanceApproved", operation: "Equals", value: false
                        }]
                    }, 
                ]
            };
            var widget = 'promo';
            var text = "On approval";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#00009b';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Actual Shopper TI 
        getActualShopperTIDemandFinance: function () {

            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Finished'
                }, {
                    operator: "or",
                    rules: [{
                        property: "ActualPromoTIShopper", operation: "Equals", value: null
                    },
                    {
                        property: "ActualPromoTIShopper", operation: "Equals", value: 0
                    }]
                },
                ]
            }
            var widget = 'promo';
            var text = "Actual Shopper TI ";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#00009b';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Promo TI cost
        getPromoTICostDemandFinance: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);

            var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
            var filter = {
                operator: "and",
                rules: [
                    {
                        operator: "or",
                        rules: [{
                            property: "ActualCostTE", operation: "Equals", value: 0
                        }, {
                            property: "ActualCostTE", operation: "Equals", value: null
                        }],
                    }, {
                        property: "EndDate", operation: "LessOrEqual", value: dateEnd
                    }
                ]
            }
            var widget = 'associatedpromosupport';
            var text = "Promo TI cost";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Non-promo TI cost
        getNonPromoTICostDemandFinance: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);

            var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
            var filter = {
                operator: "and",
                rules: [
                    {
                        operator: "or",
                        rules: [{
                            property: "ActualCostTE", operation: "Equals", value: 0
                        }, {
                            property: "ActualCostTE", operation: "Equals", value: null
                        }],
                    }, {
                        property: "EndDate", operation: "LessOrEqual", value: dateEnd
                    }

                ]
            }
            var widget = 'associatednonpromosupport';
            var text = "Non-promo TI cost";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Demand planning Plan
        getDemandPlanningPlanDemandFinance: function () {

            var filter = {
                operator: "and",
                rules: [
                    {
                        property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                    },
                    {
                        operator: "or",
                        rules: [{
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: false
                                },
                                {
                                    operator: "or",
                                    rules: [
                                        {
                                            property: "PlanPromoLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoLSV", operation: "Equals", value: null
                                        }, {
                                            property: "PlanPromoBaselineLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoBaselineLSV", operation: "Equals", value: null
                                        }, {
                                            property: "PlanPromoUpliftPercent", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoUpliftPercent", operation: "Equals", value: null
                                        }
                                    ]
                                }
                            ]
                        }, {
                            operator: "and",
                            rules: [
                                {
                                    property: "InOut", operation: "Equals", value: true
                                },
                                {
                                    operator: "or",
                                    rules: [
                                        {
                                            property: "PlanPromoLSV", operation: "Equals", value: 0
                                        }, {

                                            property: "PlanPromoLSV", operation: "Equals", value: null
                                        }
                                    ]
                                }
                            ]
                        }],
                    },

                ]
            };
            var widget = 'promo';
            var text = "Demand planning Plan";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //Demand planning Fact
        getDemandPlanningFactDemandFinance: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, -50);
            date.setHours(23);
            date.setMinutes(59);
            date.setSeconds(59);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'Finished'
                }, {
                    operator: "or",
                    rules: [{
                        property: "ActualPromoLSV", operation: "Equals", value: null
                    },
                    {
                        property: "ActualPromoLSV", operation: "Equals", value: 0
                    }]
                },
                {

                    property: "EndDate", operation: "LessThan", value: date
                }

                ]
            }
            var widget = 'promo';
            var text = "Demand planning Fact";
            var panel = 'panel2';
            var image = 'closednew.png';
            var color = '#00dabe';
            var buttonColor = '#e6fcf9';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
        },
        //CMManager
        getTimeCriticalCMManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.HOUR, 48);
            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                    }, {
                        operator: "or",
                        rules: [{
                            property: "IsCMManagerApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: false
                        }]
                    }
                ]
            };

            var widget = 'promo';
            var text = "Time Critical";
            var panel = 'panel1';
            var style = {
                styleStandart: 'panel-time-critical-standart',
                styleZero: 'panel-time-critical-zero'
            }
            var image = 'time_criticalnew.png';
            var color = '#e1312c';
            var buttonColor = '#feeded';
            return { filter: filter, widget: widget, text: text, panel: panel, style: style, image: image, color: color, buttonColor: buttonColor };
        },
        //Needs my approval
        getNeedsMyApprovalCMManager: function () {
            var date = new Date();
            date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);

            date = Ext.Date.add(date, Ext.Date.DAY, 8 * 7);

            var filter = {
                operator: "and",
                rules: [{
                    property: "PromoStatusName", operation: "Equals", value: 'On Approval'
                },
                {
                    property: "DispatchesStart", operation: "LessThan", value: date
                    }, {
                        operator: "or",
                        rules: [{
                            property: "IsCMManagerApproved", operation: "Equals", value: null
                        },
                        {
                            property: "IsCMManagerApproved", operation: "Equals", value: false
                        }]
                    }
                ]
            };
            var widget = 'promo';
            var text = "On approval";
            var panel = 'panel1';
            var image = 'workflownew.png';
            var color = '#0e0d9e';
            var buttonColor = '#eef2fc';
            return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };

    },

    //CustomerMarketing
    //Production cost
    getProductionCostCustomerMarketing: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
        date.setHours(23);
        date.setMinutes(59);
        date.setSeconds(59);

        var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
        var filter = {
            operator: "and",
            rules: [
                {
                    operator: "or",
                    rules: [{
                        property: "ActualCostTE", operation: "Equals", value: 0
                    }, {
                        property: "ActualCostTE", operation: "Equals", value: null
                    }],
                }, {
                    property: "EndDate", operation: "LessOrEqual", value: dateEnd
                }

            ]
        }
        var widget = 'associatedcostproduction';
        var text = "Production cost";
        var panel = 'panel1';
        var image = 'closednew.png';
        var color = '#00dabe';
        var buttonColor = '#e6fcf9';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    //BTL cost
    getBTLCostCustomerMarketing: function () {
        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);
        date.setHours(23);
        date.setMinutes(59);
        date.setSeconds(59);

        var dateEnd = Ext.Date.add(date, Ext.Date.DAY, -1);
        var filter = {
            operator: "and",
            rules: [
                {
                    operator: "or",
                    rules: [{
                        property: "ActualBTLTotal", operation: "Equals", value: 0
                    }, {
                        property: "ActualBTLTotal", operation: "Equals", value: null
                    }],
                }, {
                    property: "EndDate", operation: "LessOrEqual", value: dateEnd
                }

            ]
        }
        var widget = 'associatedbtlpromo';
        var text = "BTL cost";
        var panel = 'panel1';
        var image = 'closednew.png';
        var color = '#00dabe';
        var buttonColor = '#e6fcf9';
        return { filter: filter, widget: widget, text: text, panel: panel, image: image, color: color, buttonColor: buttonColor };
    },
    getCard: function (window) {
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var view = window.up('userdashboard');
        var mask = new Ext.LoadMask(view, { msg: "Please wait..." });

         
        mask.show();
        var me = this;
        var userrole = breeze.DataType.String.fmtOData(currentRole);

        var parameters = { userrole: userrole };

        App.Util.makeRequestWithCallback('Promoes', 'GetUserDashboardsCount', parameters, function (data) {

            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value); 
                var buttons;
                for (var i in result) {
                    var method = "get" + i + currentRole; 
                    if (me[method] != undefined) {
                        buttons = me[method]();
                        var button = Ext.widget('userdashboadpanel');
                        button.widget = buttons.widget;
                        button.filter = buttons.filter; 

                        if (result[i] != 0) {
                            button.down('#CountLabel').setText(result[i]);

                        }

                        button.down('button').style = 'background-color:' + buttons.buttonColor;
                        button.down('#buttonPanel').style = 'background-color:' + buttons.buttonColor;
                        button.down('#buttonArrow').style = 'background-color:' + buttons.buttonColor; 
                        button.down('#glyphRight').style = 'background-color:' + buttons.color;
                        button.down('#glyphRight').setSrc('/Bundles/style/images/' + buttons.image); 
                        if (buttons.style) {
                            button.width = '50.5%';

                            if (result[i] == 0) {
                                button.down('#CountLabel').addCls('panel-time-critical-zero');
                                button.down('#CountLabel').setVisible(false); 
                                button.down('#glyphOk').visible = true; 
                                button.down('#glyphRight').style = 'background-color:' + buttons.color; 
                                button.down('#glyphRight').style = 'background-color:#cbd5df';
                                button.down('button').style = 'background-color:#fafafa';
                                button.down('#buttonArrow').style = 'background-color:#fafafa';
                                button.down('#buttonPanel').style = 'background-color:#fafafa';
                            } else {
                                button.down('#CountLabel').addCls('panel-time-critical-standart');
                            }
                        } 

                        button.down('button').setText(buttons.text);
                        if (view.down('#' + buttons.panel))
                            view.down('#' + buttons.panel).add(button);
                        Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', buttons.color); 
                        if (buttons.style) {
                            if (result[i] == 0) {
                                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('color', '#fafafa');
                            } else {
                                Ext.get(button.down('#buttonArrow').id + '-btnIconEl').setStyle('padding-left', (button.down('#buttonArrow').getWidth() *0.5)+'px');
                            }
                        }
                    }
                }
                mask.hide();
            }
        }, function (data) {
               mask.hide();
            console.log(data);
        });
    }
});
