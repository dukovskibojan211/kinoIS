﻿using KinoIS.Domain.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoIs.Repository.Interface
{
    public interface TicketInShoppingCartRepository
    {
        List<TicketInShoppingCart> findAll();
        List<TicketInShoppingCart> findAllByShoppingCartId(Guid id);
    }
}