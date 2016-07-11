﻿using System;
using AppServiceHelpers;

using FormsSample.Models;
using FormsSample.DataStores;
using Xamarin.Forms;
using System.Threading.Tasks;
using AppServiceHelpers.Abstractions;
using System.Collections.ObjectModel;


namespace FormsSample.ViewModels
{
	public class ToDosViewModel : BaseViewModel
    {
        IEasyMobileServiceClient client;
        public ToDosViewModel(IEasyMobileServiceClient client)
        {
            this.client = client;

			Todos = new ConnectedObservableCollection<ToDo>(client.Table<ToDo>());
        }

		ConnectedObservableCollection<ToDo> todos;
		public ConnectedObservableCollection<ToDo> Todos
		{
			get { return todos; }
			set { todos = value; OnPropertyChanged("Todos"); }
		}

		Command refreshCommand;
		public new Command RefreshCommand
		{
			get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
		}

		new async Task ExecuteRefreshCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				await Todos.Refresh();
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}
    }
}