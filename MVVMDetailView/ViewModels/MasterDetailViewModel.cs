using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMDetailView.ViewModels
{
    //Template for all Master Detail ViewModels to inherit from
    public abstract class MasterDetailViewModel<T>: ObservableRecipient
    {
        private readonly ObservableCollection<T> items = new();
        public ObservableCollection<T> Items
        {
            get { if (filter is null)
                {
                    return items;
                }
                else { 
                    var results = from item in items
                                  where this.ApplyFilter(item, filter)
                                  select item;
                    return new ObservableCollection<T>(results);
                }
            }
        }

        private T current;
        public T Current { 
            get => current;
            set { 
                SetProperty(ref current, value);
                OnPropertyChanged(nameof(HasCurrent));
            }
        }

        //Whenever filter changes, notify that Items should also change
        //Store the original Current. Notify Items to change. Keep current item as original Current
        private string filter;
        public string Filter
        {
            get => filter;
            set
            {
                var current = Current;
                SetProperty(ref filter, value);
                OnPropertyChanged(nameof(Items));

                if (current is not null)
                {
                    Current = current;
                }
            }
        }
        
        public bool HasCurrent => this.Current is not null;
        public abstract bool ApplyFilter(T item, string filter);

        //Create, Update & Delete items. Call OnPropertyChanged(nameof(Items)) on adding to items, as .Add() does not trigger a setter/notification
        public virtual T AddItem(T item)
        {
            items.Add(item);
            if (filter is not null)
            {
                OnPropertyChanged(nameof(Items));
            }
            return item;
        }

        public virtual T UpdateItem(T newItem, T original)
        {
            var hasCurrent = this.HasCurrent;

            var i = items.IndexOf(original);
            items[i] = newItem;

            if (filter is not null)
            {
                OnPropertyChanged(nameof(Items));
            }

            if (hasCurrent && !HasCurrent)
            {
                //Restore current item
                Current = newItem;
            }
            return newItem;
        }

        public virtual void DeleteItem(T item)
        {
            items.Remove(item);
            if (filter is not null)
            {
                OnPropertyChanged(nameof(Items));
            }
        }
    }
}
