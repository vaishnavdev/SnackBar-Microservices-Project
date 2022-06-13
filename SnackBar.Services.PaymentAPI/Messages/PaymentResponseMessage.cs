﻿using SnackBar.MessageBus;

namespace SnackBar.Services.PaymentAPI.Messages
{
    public class PaymentResponseMessage : BaseMessage
    {
        public int OrderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
