//RecipesViewer.xaml.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;

namespace YtoX
{
    public partial class RecipesViewer : UserControl
    {
        public const string RecipeHelpText = "Scope means minimum duration between successive notifications shown by recipes. ";

        #region Constructors

        public RecipesViewer()
        {
            InitializeComponent();

            HelpText.ToolTip = RecipeHelpText;
            ReloadButton_Click(null, null);
        }

        #endregion

        #region Internal-Methods

        private void LoadRecipesTree()
        {
            RecipesTree.Items.Clear();
            foreach (var group in Repository.Instance.Groups)
            {
                TreeViewItem groupNode = new TreeViewItem();
                groupNode.IsExpanded = false;
                groupNode.Header = group;
                groupNode.HeaderTemplate = (DataTemplate)this.Resources["RecipeGroupTemplate"];

                foreach (var recipe in group.Recipes)
                {
                    if (recipe.IsAvailable)
                    {
                        TreeViewItem recipeNode = new TreeViewItem();
                        recipeNode.Header = recipe;
                        recipeNode.HeaderTemplate = (DataTemplate)this.Resources["RecipeTemplate"];
                        groupNode.Items.Add(recipeNode);
                    }
                }

                RecipesTree.Items.Add(groupNode);
            }

            //expand all groups separately to visually indicate loading-complete
            UIHelper.Run(() =>
            {
                foreach (TreeViewItem item in RecipesTree.Items)
                {
                    item.IsExpanded = true;
                }
            }, 300);
        }

        private void UpdateEnabledCount()
        {
            int available = 0, enabled = 0, total = 0;
            foreach (var group in Repository.Instance.Groups)
            {
                available += group.Recipes.Count(recipe => recipe.IsAvailable);
                enabled += group.Recipes.Count(recipe => recipe.IsEnabled);
                total += group.Recipes.Count;
            }
            StatusTextBox.Text = enabled + "/" + available + " Recipes Enabled";
        }

        #endregion

        #region Control-Events

        private void HelpText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = RecipeHelpText;
            toolTip.IsOpen = true;

            UIHelper.Run(() => toolTip.IsOpen = false, 4000);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var source = ((CheckBox)sender).DataContext;
            if (source is Recipe)
            {
                ((Recipe)source).IsEnabled = true;
            }

            UpdateEnabledCount();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var source = ((CheckBox)sender).DataContext;
            if (source is Recipe)
            {
                ((Recipe)source).IsEnabled = false;
            }

            UpdateEnabledCount();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            RecipesTree.IsEnabled = false;
            LoadRecipesTree();
            UpdateEnabledCount();
            RecipesTree.IsEnabled = true;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RecipesTree.IsEnabled = ReloadButton.IsEnabled = SaveButton.IsEnabled = false;
                await Task.Run(() => Repository.Instance.Save());
                RecipesTree.IsEnabled = ReloadButton.IsEnabled = SaveButton.IsEnabled = true;

                MessageBox.Show("Recipes Saved.", Constants.APP_ID, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
