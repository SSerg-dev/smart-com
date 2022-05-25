using System;
using Persist;
using Module.Persist.TPM.Model.TPM;
using System.Collections.Generic;

namespace Module.Persist.TPM.PromoStateControl
{
    public partial class PromoStateContext : IPromoState, IDisposable
    {
        private PromoStateContext()
        {
            _undefinedState = new UndefinedState(this);
            _draftState = new DraftState(this);
            _draftPublishedState = new DraftPublishedState(this);
            _onApprovalState = new OnApprovalState(this);
            _approvedState = new ApprovedState(this);
            _plannedState = new PlannedState(this);
            _cancelledState = new CancelledState(this);
            _startedState = new StartedState(this);
            _finishedState = new FinishedState(this);
            _closedState = new ClosedState(this);
            _deletedState = new DeletedState(this);
        }

        public PromoStateContext(DatabaseContext databaseContext, Promo currentPromoModel) : this()
        {
            if (databaseContext != null)
            {
                dbContext = databaseContext;

                if (currentPromoModel != null && currentPromoModel.PromoStatus != null)
                {
                    Model = currentPromoModel;

                    switch (currentPromoModel.PromoStatus.SystemName)
                    {
                        case "Draft":
                            State = _draftState;
                            break;
                        case "DraftPublished":
                            State = _draftPublishedState;
                            break;
                        case "OnApproval":
                            State = _onApprovalState;
                            break;
                        case "Approved":
                            State = _approvedState;
                            break;
                        case "Planned":
                            State = _plannedState;
                            break;
                        case "Cancelled":
                            State = _cancelledState;
                            break;
                        case "Started":
                            State = _startedState;
                            break;
                        case "Finished":
                            State = _finishedState;
                            break;
                        case "Closed":
                            State = _closedState;
                            break;
                        case "Deleted":
                            State = _deletedState;
                            break;
                        default:
                            throw new Exception("Invalid input parameters");
                    }
                }
                else
                {
                    State = _undefinedState;
                }
            }
            else
            {
                throw new Exception("Database сontext is not defined");
            }
        }

        private readonly IPromoState _undefinedState;
        private readonly IPromoState _draftState;
        private readonly IPromoState _draftPublishedState;
        private readonly IPromoState _onApprovalState;
        private readonly IPromoState _approvedState;
        private readonly IPromoState _plannedState;
        private readonly IPromoState _cancelledState;
        private readonly IPromoState _startedState;
        private readonly IPromoState _finishedState;
        private readonly IPromoState _closedState;
        private readonly IPromoState _deletedState;

        public IPromoState State { get; private set; }

        private readonly DatabaseContext dbContext;

        private Promo Model = null;

        public string GetName()
        {
            return State.GetName();
        }

        public List<string> GetRoles()
        {
            return State.GetRoles();
        }

        public Promo GetModel()
        {
            return State.GetModel();
        }

        public Dictionary<string, List<string>> GetAvailableStates()
        {
            return State.GetAvailableStates();
        }

        public bool ChangeState(Promo promoModel, string userRole, out string message)
        {
            return State.ChangeState(promoModel, userRole, out message);
        }

        public bool ChangeState(Promo promoModel, PromoStates promoStates, string userRole, out string message)
        {
            return State.ChangeState(promoModel, promoStates, userRole, out message);
        }

        public IPromoState GetPromoState(string statusName)
        {
            IPromoState promoState = _undefinedState;

            if (!String.IsNullOrEmpty(statusName))
            {
                switch (statusName)
                {
                    case "Draft":
                        promoState = _draftState;
                        break;
                    case "DraftPublished":
                        promoState = _draftPublishedState;
                        break;
                    case "OnApproval":
                        promoState = _onApprovalState;
                        break;
                    case "Approved":
                        promoState = _approvedState;
                        break;
                    case "Planned":
                        promoState = _plannedState;
                        break;
                    case "Cancelled":
                        promoState = _cancelledState;
                        break;
                    case "Started":
                        promoState = _startedState;
                        break;
                    case "Finished":
                        promoState = _finishedState;
                        break;
                    case "Closed":
                        promoState = _closedState;
                        break;
                    case "Deleted":
                        promoState = _deletedState;
                        break;
                    default:
                        promoState = _undefinedState;
                        break;
                }
            }

            return promoState;
        }

        Boolean isDisposed = false;

        public void Dispose()
        {
            ReleaseResources(true);
            GC.SuppressFinalize(this);
        }

        protected void ReleaseResources(bool isFromDispose)
        {
            if (!isDisposed)
            {
                if (isFromDispose)
                {
                }
            }
            isDisposed = true;
        }
        ~PromoStateContext()
        {
            ReleaseResources(false);
        }
    }
}
